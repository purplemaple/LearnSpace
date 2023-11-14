using System.Diagnostics;

CancellationTokenSource cts = new(TimeSpan.FromSeconds(1));

CancellationToken token = cts.Token;

/* 注意：这里在创建异步任务前就将 token 取消掉
 * 结果是程序直接抛异常，不会进到 Task.Run() 里面
 */
cts.Cancel();

var sw = Stopwatch.StartNew();
try
{
    /*
     * 这种写法的实质是：Task.Run(Action action, CancellationToken token)
     * 可见，这个 token 其实并没有传到 Action 内，因此这个 token 被取消时，影响不到 Action 内部，任务将继续执行
     */
    await Task.Run(() =>
    {
        for (int i = 0; i < 1000; i++)
        {
            Thread.Sleep(1000);
            Console.WriteLine("Poling...");
        }
    }, token);
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