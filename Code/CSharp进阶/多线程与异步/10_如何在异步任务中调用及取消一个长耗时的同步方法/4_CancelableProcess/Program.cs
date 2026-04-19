using System.Diagnostics;

namespace _4_CancelableProcess;

//4.4 主程序启动项，创建 CancelableProcessTask 实例，通过它开辟单独进程运行第三方类库
internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Start task");
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(7));
        var start = Stopwatch.GetTimestamp();

        //传入要运行的第三方类库的路径和参数，这里通过参数区分实际执行的方法
        var processTask = new CancelableProcessTask(@".\4_ThirdPartyClassLibWrapper.exe", "uncancelable");
        try
        {
            var task = processTask.RunAsync(cts.Token);
            await task;
            Console.WriteLine("Task ended successfully");
        }
        catch (OperationCanceledException)
        {
            //进程被杀，成功取消任务，这里的 OperationCanceledException 是由 TaskCompletionSource 的 SetCanceled 方法抛出的
            Console.WriteLine("Task was canceled");
        }
        catch (Exception ex)
        {
            //进程被杀，但发生了异常，取消失败，这里的异常可能是 Process.Kill 方法抛出的，也可能是 TaskCompletionSource 的 SetException 方法抛出的
            Console.WriteLine($"Task was killed with exception: {ex.Message}");
        }

        var elapsed = Stopwatch.GetElapsedTime(start);
        Console.WriteLine($"Elapsed time: {elapsed.TotalSeconds}");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}
