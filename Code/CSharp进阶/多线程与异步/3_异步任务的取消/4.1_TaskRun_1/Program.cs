using System.Diagnostics;

CancellationTokenSource cts = new(TimeSpan.FromSeconds(1));

CancellationToken token = cts.Token;


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
            //这样写就可以取消异步任务，因为我们定义 token 时是在 Try 语句外部，因此这里调用的其实是外部作用域的 token，仍然和下面传入的没关系
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            Thread.Sleep(1000);
            Console.WriteLine("Poling...");
        }
    }, token);

    /* 错误写法：Task.Run 的参数是一个 Action 或 Func 委托，而不是一个 lambda 表达式。
     * 将 CancellationToken 写在括号内的话，这就不再代表 Action 了
     */
    /*await Task.Run((CancellationToken token) =>
    {

    });*/
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