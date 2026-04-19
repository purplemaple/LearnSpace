//声明一个 CancellationTokenSource，用于取消异步任务
CancellationTokenSource cts = new();

try
{
    //创建异步任务时带上 CancellationToken 参数
    Task task = Task.Delay(100000, cts.Token);

    //2秒后取消异步任务
    Thread.Sleep(2000);
    cts.Cancel();

    await task;
}
catch (TaskCanceledException)   //异步任务被取消时会抛出异常
{
    Console.WriteLine("Task canceles");
}
finally
{
    // CancellationTokenSource 类实现了 IDisposable 接口，是可以被释放的，因此使用完记得释放
    cts.Dispose();
}
