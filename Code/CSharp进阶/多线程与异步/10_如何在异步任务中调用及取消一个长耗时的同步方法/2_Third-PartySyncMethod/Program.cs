using System.Diagnostics;

namespace _2_Third_PartySyncMethod;

//部分场景（如在调用老旧类库的同步方法时）无法修改同步方法的形参，从而无法使用  CancellationToken 进行取消
internal class Program
{
    static async Task Main(string[] args)
    {
        //2.1 尝试使用 Task.Run 包装同步方法并传入 CancellationToken（此方法无效）
        //注：Run 可以传入 CancellationToken ，但是其只会在调用委托前进行检查，委托开始执行后凭此 token 无法取消
        //创建一个3秒后取消的 token
        var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));

        //实际上 LongRunningJob 不会被取消
        await Task.Run(LongRunningJob, cts.Token);


        //2.2 尝试使用超时机制（此方法无效）

        //var start = Stopwatch.StartNew    //这种写法会引入内存开销
        //最高效的方式：使用 GetTimestamp 和 GetElapsedTime 组合
        var start = Stopwatch.GetTimestamp();

        var task1 = Task.Run(LongRunningJob);
        var task2 = Task.Delay(3000);

        await Task.WhenAny(task1, task2);

        var elapsed = Stopwatch.GetElapsedTime(start);

        Console.WriteLine($"Elapsed time: {elapsed}");

        Console.WriteLine("Done.");
        Console.ReadKey();
        //结果3秒后输出"Done."，但`LongRunningJob`实际上仍在运行，2秒后依旧输出"Long running job completed"
        //原因：超时机制实际上只会提前返回，并不会取消原任务


        void LongRunningJob()
        {
            Thread.Sleep(5000);
            Console.WriteLine("Long running job completed");
        }
    }
}
