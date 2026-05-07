
## 1. 问题背景

有时候会遇到这样的情形：

有多个异步任务，这些任务之间没有依赖关系，但是我们需要等待所有任务都完成后再继续执行后续的操作。我们唯一知道的，就是这些任务的数量。

> 举个例子：我们现在有三个 IO 相关的异步任务。这些任务的先后顺序是不确定的，并且这些任务也不必同时发起，但是我们需要等待这三个任务都完成后再继续执行后续的操作。

对于最普通的等待多个异步任务，我们首先肯定会想到使用 `Task.WhenAll` 方法。但是 `Task.WhenAll` 现在并不能满足我们的需求，因为它需要能够立刻获取到所有任务的集合。并且因为我们希望在每个异步任务的中间某个环节去等待其他任务的完成，而并不是所有异步任务都会在同一时间点发起，所以这就产生了一个矛盾。

---

## 2. 引入 AsyncBarrier

`AsyncBarrier`是一个非常轻量的类，可以帮助我们等待并同步多个异步任务。这个类由`Microsoft.VisualStudio.Threading`提供。

[AsyncBarrier源代码](https://github.com/microsoft/vs-threading/blob/main/src/Microsoft.VisualStudio.Threading/AsyncBarrier.cs)

实际使用时不推荐直接将这个类库引入到项目中，因为这个库本身是一个非常庞大的库，而且里面还包含了一些代码分析器（Code Analyzers），会给我们的项目添加一些恼人的“波浪线”。因此建议直接将`AsyncBarrier`源代码复制到项目中

对应示例代码：[AsyncBarrier.cs](../../../Code/CSharp进阶/多线程与异步/11_利用AsyncBarrier实现多个异步任务的同时完成/1_AsyncBarrier/AsyncBarrier.cs)
```csharp
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1_AsyncBarrier;

/// <summary>
/// An asynchronous barrier that blocks the signaler until all other participants have signaled.
/// </summary>
public class AsyncBarrier
{
    /// <summary>
    /// The number of participants being synchronized.
    /// </summary>
    private readonly int participantCount;

    /// <summary>
    /// The set of participants who have reached the barrier, with their awaiters that can resume those participants.
    /// </summary>
    private readonly Stack<Waiter> waiters;

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
                return new    ValueTask(tcs.Task);
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
```

上述代码中不难观察到以下几点：
	1. 它内部有一个`participantCount`字段，表示参与者的数量，另外还有一个`Stack<Waiter>`，用来存储所有等待的参与者（因为是栈，所以遵循先入后出原则）；
	2. 它只有一个公开的方法`SignalAndWait`，表示调用者现在要进入等待状态。在这个方法中：
		-  首先，它会判断当前等待的参与者数量是否等于预期的参与者数量。如果是，那么就将等待器逐个从 `Stack` 中弹出并唤醒；
		- 如果不是，那么就创建一个新的 `TaskCompletionSource`，并将其存入 `Stack` 中，然后返回这个 `TaskCompletionSource` 的 `Task` 给参与者用于 `await`。
		-  当所有参与者都到齐后，`SignalAndWait` 方法会返回一个已完成的 `ValueTask`，这时候所有参与者都可以继续执行后续的操作。
	
> 这里其实还有一个小细节，就是 `Stack` 的容量是 `participantCount - 1`。这是因为我们并不需要将最后一个参与者也入栈。毕竟，当“倒数第一”到达终点时，我们就可以直接宣告比赛结束了。

---

## 3. 使用 AsyncBarrier 

这里借助`CommunityToolkit.Mvvm` 这个库来写一个视图模型（ViewModel）：

```csharp
partial class MainViewModel : ObservableObject
{
    public ObservableCollection<string> Results { get; } = new();

    private AsyncBarrier _asyncBarrier = new(3);

    [RelayCommand]
    async Task FirstJobAsync(CancellationToken token)
    {
        await Task.Delay(1500, token);
        Results.Add("First job completed. Waiting for async barrier...");
        await _asyncBarrier.SignalAndWait(token);
        Results.Add("First job completed.");
    }

    [RelayCommand]
    async Task SecondJobAsync(CancellationToken token)
    {
        await Task.Delay(1500, token);
        Results.Add("Second job completed. Waiting for async barrier...");
        await _asyncBarrier.SignalAndWait(token);
        Results.Add("Second job completed.");
    }

    [RelayCommand]
    async Task ThirdJobAsync(CancellationToken token)
    {
        await Task.Delay(1500, token);
        Results.Add("Third job completed. Waiting for async barrier...");
        await _asyncBarrier.SignalAndWait(token);
        Results.Add("Third job completed.");
    }
}
```

这里我们定义了三个异步方法 `FirstJobAsync`、`SecondJobAsync` 和 `ThirdJobAsync`，它们分别模拟了三个异步任务。这三个任务之间没有依赖关系，但是我们希望在它们都完成后再继续执行后续的操作。

我们在类中声明了一个 `AsyncBarrier` 字段，然后让这三个任务都调用它的 `SignalAndWait` 方法，这样就可以保证这三个任务都进入到`SignalAndWait`时才会被放行执行后面的操作

实际运行代码，我们可以发现确实达到了我们想要实现的效果。这三个按钮可以让用户以任意的顺序及时间间隔进行点击，并且每个任务接近完成的时候，都会进入等待状态。只有当所有任务都完成后，我们才会看到所有任务都已完成的提示。

更棒的是，`AsyncBarrier` 还可以重复使用。毕竟它底层只是一个 `Stack`。我们在等待时会入栈，等待完成后会出栈，最终使它回归初始状态。这样我们就可以在界面中反复实验这一现象。

---
## 4. 取消任务

### 4.1 实现取消逻辑

现在我们更进一步，为这些异步任务添加取消功能。并且希望`AsyncBarrier`在使用时才初始化。那么，首先我们可以添加`InitAllJobs` 与 `FinishJobs` 两个方法：

```csharp
/*
 * 补充点一: MemberNotNull 这个特性出自 System.Diagnostics.CodeAnalysis 命名空间，表示在调用该方法后，指定的成员将不为 null，比直接用 ! 更加安全和明确。
 *      其中 System.Diagnostics.CodeAnalysis 常用于代码分析和静态检查。
 */
[MemberNotNull(nameof(_asyncBarrier))]
void InitJobs()
{
    if (_asyncBarrier is null)
    {
        _asyncBarrier = new AsyncBarrier(3);
        Results.Clear();
    }
}

void FinishJobs(bool success = true)
{
    if (_asyncBarrier is not null)
    {
        _asyncBarrier = null;
        if (success) 
            Results.Add("All jobs are completed.");
        else
            Results.Add("All jobs are cancelled.");
    }
}
```

这两个方法分别用于初始化任务与结束任务。在初始化任务时，我们会创建一个新的 `AsyncBarrier` 实例，并清空 `Results` 集合。在结束任务时，我们会将 `AsyncBarrier` 实例置空，并根据是否成功完成任务来添加提示信息。

> 这其实也是比较推荐的使用 `AsyncBarrier` 的方式。虽然我们前面说了，它可以被重复使用。但是观察它的源代码会发现，它非常轻量，也不需要担心资源释放的问题，因为我们大可以每次使用的时候都实例化一个新的出来。毕竟这样还有一个好处，就是每次我们都可以根据实际情况去调整它的 `participantCount`。

接下来我们就可以实现取消按钮，同时在每个异步任务中添加取消逻辑。以 `FirstJobAsync` 为例：

```csharp
/*
 * 补充点二：RelayCommand 有不少重载
 *      其中 IncludeCancelCommand 参数设为 true 会自动生成一个对应的 MethodNameCancelCommand，使用这个 CancelComman.Execute 即可取消原方法(需要原方法携带 CancellationToken 参数)
 */
[RelayCommand]
async Task FirstJobAsync(CancellationToken token)
{
    InitJobs();
	/*
	 * 补充点三：选中代码块按 Ctrl + k + s 可以快速生成代码片段
	 */
    try
    {
        await Task.Delay(1200, token);
        Results.Add("First job completed. Waiting for async barrier...");

        await _asyncBarrier.SignalAndWait(token);

        FinishJobs();
    }
    catch (TaskCanceledException)
    {
        Results.Add("First job was canceled.");
        FinishJobs(false);
    }
}

[RelayCommand]
void CancelAllJobs()
{
    //使用 [RelayCommand(IncludeCancelCommand = true)] 即可生成此方法用于取消任务
    //if (FirstJobCommand.IsRunning) FirstJobCancelCommand.Execute(null);
    //if (SecondJobCommand.IsRunning) SecondJobCancelCommand.Execute(null);
    //if (ThirdJobCommand.IsRunning) ThirdJobCancelCommand.Execute(null);

    //这里也可以直接调用Command的 Cancel() 来取消任务，效果是一样的
    if (FirstJobCommand.IsRunning) FirstJobCommand.Cancel();
    if (SecondJobCommand.IsRunning) SecondJobCommand.Cancel();
    if (ThirdJobCommand.IsRunning) ThirdJobCommand.Cancel();
}
```

这样我们就实现了想要的结果。此时`XAML`代码如下：

```xml
<Window.DataContext>
    <local:MainViewModel />
</Window.DataContext>
<DockPanel>
    <DockPanel DockPanel.Dock="Bottom" LastChildFill="False">
        <DockPanel.Resources>
            <Style TargetType="Button">
                <Setter Property="Width" Value="75" />
                <Setter Property="Padding" Value="10, 5" />
                <Setter Property="Margin" Value="5" />
            </Style>
        </DockPanel.Resources>
        <Button Content="Job1" Command="{Binding FirstJobCommand}" />
        <Button Content="Job2" Command="{Binding SecondJobCommand}" />
        <Button Content="Job3" Command="{Binding ThirdJobCommand}" />
        <Button Content="Cancel" Command="{Binding CancelAllJobsCommand}" DockPanel.Dock="Right" />
    </DockPanel>
    <ListBox ItemsSource="{Binding Results}" />
</DockPanel>
```

### 4.2 动态调节取消按钮的可用性

完成前面的操作后我们发现，`Cancel` 按钮在任何时候都是可用的。这是因为我们没有正确处理它的`ICommand`的`CanExecute`方法。

此时可以使用两种方法：
1. 使用`RelayCommand`的`CanExecute`属性指明监听的对象，同时当该对象改变时手动触发`NotifyCanExecuteChanged`通知`RelayCommand`
2. 使用`[NotifyCanExecuteChangedFor(nameof())]`特性标注属性，当该属性发生变化时自动通知`RelayCommand`（`RelayCommand`对应的`CanExecute`逻辑不能少）

方法一：修改`CancelAllJobs`，同时在`InitJobs`和`FinishJobs`中手动触发更新事件

```csharp
/*
 * 补充四：RelayCommand 的 CanExecute 可以直接绑定一个 bool 方法/属性，这样当 Command 的 CanExecute 状态改变时 UI 会自动更新对应的按钮状态
 *      注意：需要调用 CanCancel.NotifyCanExecuteChanged() 来触发通知，想要不手动通知，可参考补充五
 */
bool CanCancel => AsyncBarrier is not null;

[RelayCommand(CanExecute = nameof(CanCancel))]
void CancelAllJobs()
{
    //使用 [RelayCommand(IncludeCancelCommand = true)] 即可生成此方法用于取消任务
    //if (FirstJobCommand.IsRunning) FirstJobCancelCommand.Execute(null);
    //if (SecondJobCommand.IsRunning) SecondJobCancelCommand.Execute(null);
    //if (ThirdJobCommand.IsRunning) ThirdJobCancelCommand.Execute(null);

    //这里也可以直接调用Command的 Cancel() 来取消任务，效果是一样的
    if (FirstJobCommand.IsRunning) FirstJobCommand.Cancel();
    if (SecondJobCommand.IsRunning) SecondJobCommand.Cancel();
    if (ThirdJobCommand.IsRunning) ThirdJobCommand.Cancel();
}

[MemberNotNull(nameof(_asyncBarrier))]
void InitJobs()
{
    if (_asyncBarrier is null)
    {
        _asyncBarrier = new AsyncBarrier(3);
        Results.Clear();
    }

	//手动触发通知
    CancelAllJobsCommand.NotifyCanExecuteChanged();
}

void FinishJobs(bool success = true)
{
    if (_asyncBarrier is not null)
    {
        _asyncBarrier = null;
        if (success) 
            Results.Add("All jobs are completed.");
        else
            Results.Add("All jobs are cancelled.");
    }

	//手动触发通知
    CancelAllJobsCommand.NotifyCanExecuteChanged();
}
```

方法二：使用`[NotifyCanExecuteChangedFor(nameof())]`标注`AsyncBarrier`属性，自动触发更新事件。其中`CancelAllJobs`与方法一相同

```csharp
/*
 * 补充五：使用 [NotifyCanExecuteChangedFor(nameof())]，在属性变更时自动通知对应的 Command
 *      这个特性需要标注在属性上, 因此这里同时使用 [ObservableProperty] 生成对应的属性
 * 
 *      逻辑: 当 AsyncBarrier 属性被设置 -> CancelAllJobsCommand.NotifyCanExecuteChanged() -> 触发 CanExecuteChanged 事件 -> 重新调用 CanExecute() -> 得到最新状态 -> 更新 UI
 */
[NotifyCanExecuteChangedFor(nameof(CancelAllJobsCommand))]
[ObservableProperty]
private AsyncBarrier? _asyncBarrier;
```

注意`NotifyCanExecuteChangedFor`只能标注在属性上，因此这里还使用了`ObservableProperty`生成自动属性，`ViewModel`中原本的字段`_asyncBarrier`引用应都更替为属性`AsyncBarrier`

### 4.3 全部修改后的完整代码：

[MainViewModel.cs](../../../Code/CSharp进阶/多线程与异步/11_利用AsyncBarrier实现多个异步任务的同时完成/1_AsyncBarrier/MainViewModel.cs)

---

## 5. 升级 AsyncBarrier：处理异常导致的等待卡死问题

前面这个版本已经可以满足正常情况下的异步任务同步需求，也可以通过 `CancellationToken` 来处理用户主动取消。

但是这里还会引发一个问题：

> 如果其中一个任务在到达 `SignalAndWait` 之前就发生异常并提前退出，那么其他已经进入 `SignalAndWait` 的任务就会一直等待。因为从 `AsyncBarrier` 的角度看，它只知道参与者总数，并不知道某个参与者已经失败退出了。

这其实也是 `Barrier` 这类同步结构很容易遇到的问题：它要求所有参与者都要到达屏障点。如果其中一个参与者在屏障之前失败，那么剩下的等待者就可能永远等不到最后一个参与者。

对应示例代码：

- 普通版本：[AsyncBarrier.cs](../../../Code/CSharp进阶/多线程与异步/11_利用AsyncBarrier实现多个异步任务的同时完成/1_AsyncBarrier/AsyncBarrier.cs)
- 升级版本：[AsyncBarrier.cs](../../../Code/CSharp进阶/多线程与异步/11_利用AsyncBarrier实现多个异步任务的同时完成/2_AsyncBarrierUpgrade/AsyncBarrier.cs)

### 5.1 升级版解决了什么问题

针对原版 `AsyncBarrier` 的升级版本，新增失败传播机制，用于解决某个任务出错提前结束导致其他任务死锁的问题。

主要改动如下：

1. 添加 `broken`、`exception` 字段，记录 Barrier 是否被 Break 以及 Break 的异常信息
2. 添加 `Break` 方法，允许外部调用以 Break Barrier，并传入异常信息
3. 在 `SignalAndWait` 方法中添加 `broken` 检查，如果 Barrier 已经被 Break 则立即返回一个失败的 Task，保证等待者能够及时收到异常信息并响应，而不是继续等待其他参与者到达 Barrier
4. 在 `Break` 方法中添加对所有等待者的通知，确保当 Barrier 被 Break 时，所有等待者都能够及时收到异常信息并响应，而不是继续等待其他参与者到达 Barrier
5. 添加 `AsyncBarrierBrokenException` 类，作为 Barrier 被 Break 时抛出的异常类型，包含一个可选的 `InnerException` 用于传递具体的错误信息

这里其实可以把它理解为一个“强一致版本”的 `AsyncBarrier`：

```csharp
/// <summary>
/// AsyncBarrier（强一致版本）
///
/// 特性：
/// 1. 所有参与者必须成功到达，否则全部失败（Fail-Fast）
/// 2. 任意异常/取消 → Barrier 立即 Broken
/// 3. 已经进入 Barrier 等待的任务会收到异常
/// 4. AsyncBarrier 被标记为 broken ,不可复用（实际上即使原来的版本我们也每次 new 一个新的 AsyncBarrier 从不复用，因此不影响）
/// </summary>
```

也就是说，原版 `AsyncBarrier` 更像是“只负责等人到齐”，而升级版 `AsyncBarrier` 则额外负责“只要有人失败，就把失败状态同步给其他所有参与者”。

### 5.2 Break 方法

升级版中新增了一个 `Break` 方法：

```csharp
public void Break(Exception? ex = null)
{
    lock (this.waiters)
    {
        if (this.broken) return;

        this.broken = true;
        this.exception = ex is AsyncBarrierBrokenException abe ? abe : new AsyncBarrierBrokenException("Barrier broken", ex);

        while (this.waiters.Count > 0)
        {
            Waiter waiter = this.waiters.Pop();
            waiter.CompletionSource.TrySetException(this.exception);
            waiter.CancellationRegistration.Dispose();
        }
    }
}
```

这个方法的作用很直接：

1. 将 `broken` 标记为 `true`
2. 保存第一个导致 Barrier Broken 的异常，供后续等待者抛出，保证失败传播的一致性
3. 把当前已经在等待的参与者全部唤醒，并让它们收到异常

其中这段注释其实已经把它的设计意图说得比较清楚了：

```csharp
/// <summary>
/// 保存第一个导致 Barrier Broken 的异常，供后续等待者抛出，保证失败传播的一致性
/// </summary>
private Exception? exception;
```

这里还使用了 `volatile`：

```csharp
// volatile 关键字确保了对 broken 字段的读写操作具有适当的内存屏障，这样当一个线程将 broken 设置为 true 时，其他线程能够立即看到这个变化，从而正确地响应 Barrier 的状态变化。
private volatile bool broken;
```

这里需要注意的是，`volatile` 只是保证可见性，并不替代 `lock`。真正对 `waiters`、`broken`、`exception` 这些状态的修改，还是要靠 `lock` 来保证一致性。

### 5.3 SignalAndWait 中的 broken 检查

升级版 `SignalAndWait` 的关键变化，是在正常等待逻辑之前先判断 Barrier 是否已经被 Break：

```csharp
/* 
 * 注意：这里 broken 检查在 CancellationTokenRegistration 注册之前，因此当发生 broken 时，任务会先抛出 AsyncBarrierBrokenException 异常，而非被取消时抛出的 TaskCanceledException 异常。
 * 因此 Barrier Broken 的优先级高于 Cancellation，不会出现某些任务收到 TaskCanceledException，而另一些任务收到 AsyncBarrierBrokenException 的竞态问题，保证了失败传播的一致性。
 */
if (this.broken && this.exception is not null)
{
    // The barrier is already broken, so fail immediately.
    return new ValueTask(Task.FromException(this.exception));
}
```

这个检查很重要。

因为有些任务并不是“先进入等待队列，再被唤醒”的；它也可能是在 Barrier 已经 Broken 之后，才刚刚走到 `SignalAndWait`。如果这里不做判断，那么这个后到的任务仍然可能继续进入等待流程。

所以升级版做了两层保护：

- 对于已经在等待的任务：由 `Break()` 主动通知
- 对于之后才调用 `SignalAndWait()` 的任务：直接返回失败的 `Task`

这样 Barrier 一旦 Broken，至少所有已经进入 Barrier 等待流程的参与者，都能观察到统一的失败状态。

### 5.4 ViewModel 中如何配合使用

升级版中，调用方在捕获到普通异常后，会主动调用 `AsyncBarrier.Break(ex)`：

```csharp
catch (Exception ex)
{
    //在新的 AsyncBarrier 中，Break 方法可以接受一个异常参数，这个异常会被传递给所有等待的任务，使它们能够了解取消的原因
    AsyncBarrier.Break(ex);
    Results.Add($"some exception happened in Second job, breaking the barrier. reason: {ex.Message}");

    await Task.Yield(); //确保在 Break 之后再触发 Cancel，这样等待的任务能先感知到 Break 的异常，再感知到 Cancel 的取消，减少竞态的发生
    _cts.Cancel(); //这里可以选择直接取消所有任务，或者让其他任务通过 AsyncBarrierBrokenException 来感知异常并自行决定是否取消
}
```

这里有两个动作：

1. 先 `Break`
2. 再 `_cts.Cancel()`

先 `Break` 的原因是，要先让已经在 Barrier 中等待的任务收到 `AsyncBarrierBrokenException`。  
再 `Cancel` 的原因是，要让那些还没走到 Barrier 的任务，也能够尽快结束。

这里的 `await Task.Yield()` 也很有意思。它的目的不是“让代码更异步”，而是：

> 确保在 Break 之后再触发 Cancel，这样等待的任务能先感知到 Break 的异常，再感知到 Cancel 的取消，减少竞态的发生

这里需要注意的是，它只能减少竞态，但不能完全消除竞态。

### 5.5 为什么还要区分 AsyncBarrierBrokenException 和 TaskCanceledException

升级版中，每个任务一般都会同时捕获这两类异常：

```csharp
catch (AsyncBarrierBrokenException ex)
{
    Results.Add($"An exception happened in Third job due to barrier broken. reason: {ex.InnerException?.Message}");
}
catch (TaskCanceledException)
{
    /* 
     * 当 AsyncBarrier 已经被 Break 时，如果正好有任务被取消，这里区分语义。
     * 根据 chatGPT 的回答，即使 IsBroken 使用了 lock 语句，但仍然不能保证时序一致性，仍有可能某个任务先于 break 获取 IsBroken 状态，走了普通的 Cancel，因此只能减少而不能避免竞态的发生
     */
    if (AsyncBarrier.IsBroken)
    {
        Results.Add("Third job was cancelled due to barrier broken.");
    }
    else
    {
        Results.Add("Third job was cancelled.");
    }
    FinishJobs(false);
}
```

这里的含义其实是：

- 如果任务已经进入 Barrier 等待，并且 Barrier 被 Break，那么它更可能收到 `AsyncBarrierBrokenException`
- 如果任务还在 `Task.Delay` 之类的可取消等待中，那么它也可能先收到 `TaskCanceledException`

所以这里区分这两种异常，不是为了追求绝对精确，而是为了尽量在语义上区分：

- 这是普通取消
- 这是 Barrier 被 Break 后的连带失败

不过从代码注释里也能看出来，这个区分只能做到“尽量准确”，不能做到“绝对不会错”。

### 5.6 为什么升级版里还引入了共享 CancellationTokenSource

升级版没有继续使用 `IncludeCancelCommand = true` 自动生成的取消命令，而是额外引入了一个共享的 `CancellationTokenSource`：

```csharp
private CancellationTokenSource? _cts;
```

初始化逻辑如下：

```csharp
[MemberNotNull(nameof(_cts))]
[MemberNotNull(nameof(AsyncBarrier))]
void InitJobs()
{
    if (_cts == null)
        _cts = new CancellationTokenSource();
    if (AsyncBarrier is null)
    {
        //初始化共享 CancellationTokenSource ，所有传入 AsyncBarrier 的 任务都使用这个源，这样在 CancelAllJobs 时可以更及时地取消所有任务
        AsyncBarrier = new AsyncBarrier(3);
        Results.Clear();
    }
}
```

取消按钮则直接调用：

```csharp
[RelayCommand(CanExecute = nameof(CanCancel))]
void CancelAllJobs()
{
    /*
     * 注：以上都是 UI Command 层取消，如果任务还没进入 await （比如 CPU 密集型操作），Cancel 不一定立即生效。
     * 因此引入一个共享的 CancellationTokenSource，在 InitJobs 时创建，在 FinishJobs 时释放，这样所有任务都共享同一个 CancellationToken，可以更及时地响应取消请求。
     */
    _cts?.Cancel();
}
```

这样做的好处是：

- 三个任务共用一个 `CancellationToken`
- 用户点击取消按钮时，可以统一取消
- 某个任务异常时，也可以顺手 `_cts.Cancel()` 让其他任务尽快结束

当然，这里也还是那个老问题：`CancellationToken` 只是协作式取消，它不是强制中断。因此它的意义在于“让其他任务尽快感知停止”，而不是“保证它们立刻停下”。

### 5.7 升级版的优劣势

#### 优点

- 解决了“某个任务在 Barrier 前失败，其他任务一直卡住”的问题
- 已经进入等待的任务可以及时收到异常信息
- 后续才调用 `SignalAndWait` 的任务也会立即失败，不会继续等待
- 失败原因可以通过 `InnerException` 继续向外传递
- 比原版更适合“所有参与者必须共同成功，否则整体失败”的场景

#### 缺点

- 代码复杂度明显高于原版
- Barrier 一旦 Broken 就不可复用
- `Break` 与 `Cancel` 的先后顺序仍然可能产生竞态，因此有些任务收到的是 `AsyncBarrierBrokenException`，有些任务收到的是 `TaskCanceledException`
- 调用方必须主动在异常时调用 `Break`，否则 Barrier 本身并不知道有人已经失败退出

### 5.8 总结

普通版 `AsyncBarrier` 更适合演示和处理“所有任务最终都会到达屏障点”的场景。

而升级版 `AsyncBarrier` 更适合下面这类情况：

- 这些异步任务是一个整体
- 任何一个参与者失败，都应该让其他参与者尽快结束等待
- 需要把失败原因传递给其他等待者

也就是说，升级版不是在让 `AsyncBarrier` “更通用”，而是在让它“更强一致”。  
它解决了原版最明显的死锁风险，但代价是实现更复杂，并且 Broken 后不再适合复用。

---

## 思考：
	1. 与标准库中提供的`Barrier`有什么不同？
	2. `Barrier`与`Task.WhenAll`有什么不同
		1. `Task.WhenAll`需要立刻传入所有任务，而`Barrier`只需指定任务总数，其内任务可以逐个添加
		2. `Task.WhenAll`不控制任务执行，只等待结束；Barrier 控制执行节奏（可以控制任务停在`SignalAndWaitAsync()`这一句），然后放行（先入后出）
		3. WhenAll 一次性使用；Barrier 支持循环使用
		4. WhenAll 相对安全；Barrier 存在死锁风险（比如某个任务异常/提前结束，没有调用SignalAndWaitAsync()）
	3. 验证：System.Threading里有AsyncBarrier但是asyncBarrier.SignalAndWait()不能传入Token
	4. `CommunityToolkit`里其实也有一个[NotifyCanExecuteChangedFor(nameof(【指定的command】))]的Attribute ,可以配合【ObservableProperty】一起挂在属性上，这样就可以不用写手动触发了。但up的例子里因为asyncBarrier只是内部字段，所以还是要手动触发一下通知的。
	5. 如果其中一个线程在Barrier之前失败退出，是否导致另一个线程在Barrier卡住，这种情况如何让另一个线程继续执行

## 参考

[使用 AsyncBarrier 来等待并同步多个异步任务](https://blog.coldwind.top/posts/use-asyncbarrier-to-sync-tasks/)

https://www.bilibili.com/video/BV1Gx4y1479f

[AsyncBarrier源代码](https://github.com/microsoft/vs-threading/blob/main/src/Microsoft.VisualStudio.Threading/AsyncBarrier.cs)
