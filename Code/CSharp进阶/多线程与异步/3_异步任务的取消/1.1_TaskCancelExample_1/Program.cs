using System.Diagnostics;

/*
 * 当只用于单一作用域时，可以直接用 using 关键字来确保用完后释放
 * 但是当声明成属性或字段时不能用 using
 * using CancellationTokenSource cts = new();
 */

CancellationTokenSource cts = new();
//拿到 Source 里的 Token
CancellationToken token = cts.Token;

var sw = Stopwatch.StartNew();
try
{
    //这里创建一个异步任务，模拟运行 3s 后被取消
    Task cancelTask = Task.Run(async () =>
    {
        await Task.Delay(3000);
        //await Task.Delay(6000);   //设为6秒后取消的话，因为 Task.Delay(5000, token) 已完成，则不会抛出异常
        cts.Cancel();
    });

    /*
     * 注：Task.Delay()中已实现对 token 的检查和响应，因此直接传入就能响应取消操作
     * 自建的 cancelTask 中没有实现，因此 cts.Cancel() 无法让 cancelTask 取消
     * 
     * cts.Cancel() 取消异步任务的实质就是将 CancellationToken 内的 IsCancellationRequested 属性设为 true
     */
    //等待两个异步任务全部完成
    await Task.WhenAll(Task.Delay(5000, token), cancelTask);
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