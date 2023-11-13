/*
 * 本项目用于演示异步任务如何超时打断
 * 方法三：.Net 6.0 新增的 WaitAsync() 扩展方法
 *      1. 异步任务后面跟上 WaitAsync() 扩展方法，并传入超时时间
 *      2. 检测到超时后 WaitAsync() 会直接抛出 TimeoutException 异常
 *      3. 捕获异常后取消任务即可
 */

CancellationTokenSource cts = new();
try
{
    /* .Net 6.0 新增的 WaitAsync() 扩展方法，其不含返回值(是这个方法无返回值，不是说使用了这个方法后原异步任务的返回值也没了)
     * WaitAsync() 不像之前的 WhenAny()，需要拿到先返回的那个 completeTask，再判断、再取消
     * 而是会直接抛出 TimeoutException 异常，因此在 catch 语句中取消任务即可
     */
    await FooAsync(cts.Token).WaitAsync(TimeSpan.FromSeconds(2));
    Console.WriteLine("Success!");
}
catch (TimeoutException)
{
    //WaitAsync() 若检测到任务超时，则直接抛出异常，在这里取消任务即可
    cts.Cancel();
    Console.WriteLine("Timeout!");
}
finally
{
    cts.Dispose();
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
