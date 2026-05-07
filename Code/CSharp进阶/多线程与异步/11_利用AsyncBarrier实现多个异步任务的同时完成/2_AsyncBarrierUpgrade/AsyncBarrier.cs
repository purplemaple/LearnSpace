using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text;

namespace _2_AsyncBarrierUpgrade;

/*
 * 针对原版 AsyncBarrier 的升级版本，新增失败传播机制，用于解决某个任务出错提前结束导致其他任务死锁的问题
 * 主要改动如下：
 *      1. 添加 broken、exception 字段，记录 Barrier 是否被 Break 以及 Break 的异常信息
 *      2. 添加 Break 方法，允许外部调用以 Break Barrier，并传入异常信息
 *      3. 在 SignalAndWait 方法中添加 broken 检查，如果 Barrier 已经被 Break 则立即返回一个失败的 Task，保证等待者能够及时收到异常信息并响应，而不是继续等待其他参与者到达 Barrier
 *      4. 在 SignalAndWait 方法中添加 CancellationToken 注册，以便在等待过程中如果发生取消，能够及时响应并从 Barrier 中移除对应的等待者，避免死锁，同时也允许外部通过取消来提前结束等待过程
 *      5. 在 Break 方法中添加对所有等待者的通知，确保当 Barrier 被 Break 时，所有等待者都能够及时收到异常信息并响应，而不是继续等待其他参与者到达 Barrier
 *      6. 添加 AsyncBarrierBrokenException 类，作为 Barrier 被 Break 时抛出的异常类型，包含一个可选的 InnerException 用于传递具体的错误信息
 *      7. 在 MainViewModel 中的 FirstJobAsync 方法中添加对 AsyncBarrierBrokenException 的捕获，以便在任务因 Barrier 被 Break 而失败时能够正确处理异常并更新 UI，同时保留对 TaskCanceledException 的捕获以处理取消的情况，并区分是因 Barrier 被 Break 还是普通取消导致的失败
 *      8. 在 MainViewModel 中添加一个 CancellationTokenSource 字段，用于在需要取消所有任务时能够统一管理取消操作，并在 FirstJobAsync 和 SecondJobAsync 方法中使用这个 CancellationTokenSource 的 Token 来响应取消请求，同时在 CancelAllJobsCommand 中调用 CancellationTokenSource.Cancel() 来触发取消操作
 *      9. 在 MainViewModel 中的 CancelAllJobsCommand 方法中添加对 AsyncBarrier 的 Break 调用，以便在取消所有任务时能够同时 Break Barrier，确保所有等待者都能够及时收到异常信息并响应，而不是继续等待其他参与者到达 Barrier
 *      10. 在 MainViewModel 中的 FirstJobAsync 和 SecondJobAsync 方法中添加对 AsyncBarrier.SignalAndWait 方法的调用，传入 CancellationToken 来响应取消请求，并在捕获异常时区分是因 Barrier 被 Break 还是普通取消导致的失败，以便能够正确处理异常并更新 UI
 */

/// <summary>
/// AsyncBarrier（强一致版本）
///
/// 特性：
/// 1. 所有参与者必须成功到达，否则全部失败（Fail-Fast）
/// 2. 任意异常/取消 → Barrier 立即 Broken
/// 3. 所有等待者全部取消且接收到异常
/// 4. AsyncBarrier 被标记为 broken ,不可复用（实际上即使原来的版本我们也每次 new 一个新的 AsyncBarrier 从不复用，因此不影响）
/// </summary>
/// <summary>
/// An asynchronous barrier that blocks the signaler until all other participants have signaled.
/// </summary>
public class AsyncBarrier
{
    private readonly int participantCount;
    private readonly Stack<Waiter> waiters;

    /// <summary>
    /// 保存第一个导致 Barrier Broken 的异常，供后续等待者抛出，保证失败传播的一致性
    /// </summary>
    private Exception? exception;

    // volatile 关键字确保了对 broken 字段的读写操作具有适当的内存屏障，这样当一个线程将 broken 设置为 true 时，其他线程能够立即看到这个变化，从而正确地响应 Barrier 的状态变化。
    private volatile bool broken;

    public bool IsBroken => broken;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncBarrier"/> class.
    /// </summary>
    /// <param name="participants">The number of participants.</param>
    public AsyncBarrier(int participants)
    {
        if (participants <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(participants),
                $"Argument {nameof(participants)} must be a positive number.");
        this.participantCount = participants;

        // Allocate the stack so no resizing is necessary.
        // We don't need space for the last participant, since we never have to store it.
        this.waiters = new Stack<Waiter>(participants - 1);
    }

    public void Break(Exception? ex = null)
    {
        lock (this.waiters)
        {
            if (this.broken) return;

            this.broken = true;
            //this.exception = ex ?? new AsyncBarrierBrokenException($"Barrier broken. reason");

            this.exception = ex is AsyncBarrierBrokenException abe ? abe : new AsyncBarrierBrokenException("Barrier broken", ex);

            while (this.waiters.Count > 0)
            {
                Waiter waiter = this.waiters.Pop();
                waiter.CompletionSource.TrySetException(this.exception);
                waiter.CancellationRegistration.Dispose();
            }
        }
    }

    /// <inheritdoc cref="SignalAndWait(CancellationToken)" />
    public Task SignalAndWait() => this.SignalAndWait(CancellationToken.None).AsTask();

    /// <summary>
    /// Signals that a participant is ready, and returns a Task
    /// that completes when all other participants have also signaled ready.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token that signals the caller's lost interest in waiting.
    /// The signal effect of the method is not canceled with the token.
    /// </param>
    /// <returns>A task which will complete (or may already be completed) when the last participant calls this method.</returns>
    public ValueTask SignalAndWait(CancellationToken cancellationToken)
    {
        lock (this.waiters)
        {
            /* 
             * 注意：这里 broken 检查在 CancellationTokenRegistration 注册之前，因此当发生 broken 时，任务会先抛出 AsyncBarrierBrokenException 异常，而非被取消时抛出的 TaskCanceledException 异常。
             * 因此 Barrier Broken 的优先级高于 Cancellation，不会出现某些任务收到 TaskCanceledException，而另一些任务收到 AsyncBarrierBrokenException 的竞态问题，保证了失败传播的一致性。
             */
            if (this.broken && this.exception is not null)
            {
                // The barrier is already broken, so fail immediately.
                return new ValueTask(Task.FromException(this.exception));
            }

            if (this.waiters.Count + 1 == this.participantCount)
            {
                // This is the last one we were waiting for.
                // Unleash everyone that preceded this one.
                while (this.waiters.Count > 0)
                {
                    Waiter waiter = this.waiters.Pop();
                    waiter.CompletionSource.TrySetResult(default);
                    waiter.CancellationRegistration.Dispose();
                }

                // And allow this one to continue immediately.
                return new ValueTask(cancellationToken.IsCancellationRequested
                    ? Task.FromCanceled(cancellationToken)
                    : Task.CompletedTask);
            }
            else
            {
                // We need more folks. So suspend this caller.
                TaskCompletionSource<EmptyStruct> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
                CancellationTokenRegistration ctr;
                if (cancellationToken.CanBeCanceled)
                {
#if NET
                    ctr = cancellationToken.Register(
                        static (tcs, ct) => ((TaskCompletionSource<EmptyStruct>)tcs!).TrySetCanceled(ct), tcs);
#else
                    ctr = cancellationToken.Register(
                        static s =>
                        {
                            var t = (Tuple<TaskCompletionSource<EmptyStruct>, CancellationToken>)s!;
                            t.Item1.TrySetCanceled(t.Item2);
                        },
                        Tuple.Create(tcs, cancellationToken));
#endif
                }
                else
                {
                    ctr = default;
                }

                this.waiters.Push(new Waiter(tcs, ctr));
                return new ValueTask(tcs.Task);
            }
        }
    }

    private readonly struct Waiter(TaskCompletionSource<EmptyStruct> completionSource, CancellationTokenRegistration cancellationRegistration)
    {
        internal readonly TaskCompletionSource<EmptyStruct> CompletionSource => completionSource;

        internal readonly CancellationTokenRegistration CancellationRegistration => cancellationRegistration;
    }
}

/// <summary>
/// An empty struct.
/// </summary>
/// <remarks>
/// This can save 4 bytes over System.Object when a type argument is required for a generic type, but entirely unused.
/// </remarks>
internal readonly struct EmptyStruct
{
    /// <summary>
    /// Gets an instance of the empty struct.
    /// </summary>
    internal static EmptyStruct Instance => default;
}

public class AsyncBarrierBrokenException : Exception
{
    public AsyncBarrierBrokenException()
    {
    }
    public AsyncBarrierBrokenException(string? message) : base(message)
    {
    }
    public AsyncBarrierBrokenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
