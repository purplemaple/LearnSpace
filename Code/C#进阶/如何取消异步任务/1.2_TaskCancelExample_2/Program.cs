using System.Diagnostics;

//1. 创建 CancellationTokenSource 时直接传入一个 TimeSpan 参数，时间到后会自动取消任务
CancellationTokenSource cts = new(TimeSpan.FromSeconds(3));
//2. 或者直接传入毫秒
//CancellationTokenSource cts = new(3000);

//3. 或者调用 CancelAfter() 方法
//cts.CancelAfter(TimeSpan.FromSeconds(3));
//cts.CancelAfter(3000);

CancellationToken token = cts.Token;

var sw = Stopwatch.StartNew();
try
{
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
