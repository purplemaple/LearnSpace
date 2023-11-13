using System.Diagnostics;

CancellationTokenSource cts = new(TimeSpan.FromSeconds(1));

CancellationToken token = cts.Token;

/*
 * 使用 Register() 给 Token 注册委托，当 Token 被取消时自行调用
 * 注：当注册多个委托时，后注册的会先执行，以确保先处理内层资源
 */
token.Register(() => Console.WriteLine("主线程：在这里做外层资源的善后"));

var sw = Stopwatch.StartNew();
try
{
    /*
     * 使用 Register() 给 Token 注册委托，当 Token 被取消时自行调用
     * 注：当注册多个委托时，后注册的会先执行，以确保先处理内层资源
     */
    token.Register(() => Console.WriteLine("Try 语句：在这里做内层资源的善后"));

    await Task.Delay(5000, token);
}
catch (TaskCanceledException e)
{
    Console.WriteLine(e);
}
finally
{
    //cts 实现了 IDisposable 接口，因此记得用完释放掉
    cts.Dispose();
}

Console.WriteLine("总共耗时：" + sw.ElapsedMilliseconds + " ms");