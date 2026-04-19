namespace _3_ThreadInterrupt;


/* 
 * 3. 使用`Thread`运行，并使用`Thread.Interrupt`打断（此方法具有问题）
 * 
 * 注：`Thread.Interrupt`并非能确保打断线程
 * 
 * 思路：
 *  使用`Thread`运行长耗时同步方法
 *  使用`Thread.Interrupt`强制打断
 *  使用信号量等方式暴露一个异步任务以供等待
 */
internal class Program
{
    static async Task Main(string[] args)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(3000));

        try
        {
            var task = new CancelableThreadTask(LongRunningJob);
            await task.RunAsync(cts.Token);
        }
        catch (TaskCanceledException)
        {
            //正确取消
            Console.WriteLine("Task was canceled");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Task failed: {e.Message}");
        }

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        void LongRunningJob()
        {
            Thread.Sleep(10000);
            Console.WriteLine("Long running job completed");
        }
    }
}


//这一段是vs自动生成的，volatile 关键字值得注意
//class InterruptibleThread
//{
//    private Thread _thread;
//    private volatile bool _interrupted;
//    public InterruptibleThread(ThreadStart start)
//    {
//        _thread = new Thread(() =>
//        {
//            try
//            {
//                start();
//            }
//            catch (ThreadInterruptedException)
//            {
//                Console.WriteLine("Thread was interrupted.");
//            }
//        });
//    }
//    public void Start() => _thread.Start();
//    public void Interrupt()
//    {
//        _interrupted = true;
//        _thread.Interrupt();
//    }
//}
