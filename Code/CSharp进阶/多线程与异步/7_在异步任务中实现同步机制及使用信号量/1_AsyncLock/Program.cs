using Nito.AsyncEx;
using System.Collections;

namespace _1_AsyncLock;

/*
 * 1. AsyncLock 是一个异步锁，允许在异步方法中使用 lock 语句来控制并发（这个锁可以同时用在同步和异步方法中）。
 * 需要导入 Nito.AsyncEx 包
 */
internal class Program
{
    static async Task Main(string[] args)
    {
        //var demo = new Demo();
        //await demo.DoJobAsync();

        var testDemo = new TestDemo();
        await testDemo.Run();
    }
}


class Demo
{
    readonly AsyncLock _lock = new();

    readonly CancellationTokenSource _cts = new();

    public async Task DoJobAsync()
    {
        //AsyncLock 也可以被取消
        using (await _lock.LockAsync(_cts.Token))
        {
            //AsyncLock 本身仍然可以使用同步锁，因此在这也可以写同步语句
            _lock.Lock();
        }
    }
}


class TestDemo
{
    AsyncLock asyncLock = new();

    DateTime start = DateTime.Now;

    List<Task<int>> tasks = [];

    public TestDemo()
    {
        // 创建多个异步任务来计算整数的平方，但这里使用异步锁确保同一时间只有一个异步任务执行
        tasks = Enumerable.Range(1, 10).Select(x => ComputeAsync(x, asyncLock)).ToList();
    }


    /// <summary>
    /// 异步计算指定整数的平方，并在计算过程中使用异步锁确保线程安全。
    /// </summary>
    /// <remarks>This method acquires the specified asynchronous lock before performing the computation. Use
    /// this method when thread-safe access is required in asynchronous scenarios.</remarks>
    public async Task<int> ComputeAsync(int x, AsyncLock asyncLock)
    {
        using (await asyncLock.LockAsync())
        {
            await Task.Delay(300);
            return x * x;
        }
    }

    public async Task Run()
    {
        var results = await Task.WhenAll(tasks);
        Console.WriteLine(string.Join(", ", results));

        var end = DateTime.Now;
        Console.WriteLine($"Elapsed: {(end - start).TotalMilliseconds:F4} ms");
    }

}
