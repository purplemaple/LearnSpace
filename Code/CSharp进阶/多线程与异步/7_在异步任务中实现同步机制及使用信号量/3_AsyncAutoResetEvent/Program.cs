namespace _3_AsyncAutoResetEvent;


/*
 * 3. AsyncAutoResetEvent 是一个异步自动重置事件，允许在异步方法中使用 AutoResetEvent 来控制线程的执行顺序。
 * 每次获取锁后会自动 reset 上锁。除非再次 set 解锁，否则后续的调用者将一直等待。
 * 需要导入 Nito.AsyncEx 或 Microsoft.VisualStudio.Threading 包（构造稍有不同）
 */

//using Nito.AsyncEx;
using Microsoft.VisualStudio.Threading;

internal class Program
{
    static async Task Main(string[] args)
    {
        var signal = new AsyncAutoResetEvent(false);

        var setter = Task.Run(async () =>
        {
            Console.WriteLine("Setter is waiting for 2 seconds...");
            await Task.Delay(2000);
            Console.WriteLine("Setter is setting the signal.");
            //释放锁
            signal.Set();
        });

        var waiter = Task.Run(async () =>
        {
            Console.WriteLine("Waiter is waiting for the signal...");
            //获取锁，同时 AsyncAutoResetEvent 会自动 reset 上锁，除非再次调用 Set 方法，否则后续的调用者将一直等待
            await signal.WaitAsync();
            Console.WriteLine("Waiter received the signal!");

            await signal.WaitAsync();
            Console.WriteLine("test 永久等待"); //这条语句永久无法执行
        });

        await Task.WhenAll(setter, waiter);
    }
}
