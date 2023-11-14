/*
 * 本项目用于演示异步任务如何超时打断
 * 方法二：用扩展方法包装 Task.WhenAny()，原理相同，但更模块化和简洁
 */

try
{
    //直接使用扩展语法写即可，两秒未完成则超时取消任务
    await FooAsync(CancellationToken.None).TimeoutAfter(TimeSpan.FromSeconds(2));
    Console.WriteLine("Success!");
}
catch (TimeoutException)
{
    Console.WriteLine("Timeout!");
}

Console.WriteLine("Done...");

async Task FooAsync(CancellationToken token)
{
    try
    {
        Console.WriteLine("Foo start...");
        await Task.Delay(5000, token);
        Console.WriteLine("Foo end...");
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Foo is Canceled...");
    }
}

/*
 * 采用扩展方式
 * 代码更模块化，更简洁
 * 仍存在的问题：方法内的 task 没有 Cancel 掉
 */
static class AsyncExtensions
{
    //传入一个任务和一个时间，如果任务时未完成则取消掉
    public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
    {
        //内部维护一个 CancellationTokenSource
        using CancellationTokenSource cts = new();
        Task completedTask = await Task.WhenAny(task, Task.Delay(timeout, cts.Token));
        if (completedTask != task)
        {
            cts.Cancel();
            throw new TimeoutException();
        }

        await task;
    }

    //含返回值的泛型写法
    /*public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using CancellationTokenSource cts = new();
        Task completedTask = await Task.WhenAny(task, Task.Delay(timeout, cts.Token));
        if (completedTask != task)
        {
            cts.Cancel();
            throw new TimeoutException();
        }

        return await task;
    }*/
}