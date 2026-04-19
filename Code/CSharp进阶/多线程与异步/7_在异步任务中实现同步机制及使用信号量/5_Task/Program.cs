namespace _5_Task;


/*
 * 5. Task 作为一个异步任务，本身就可以 await 来控制并发，并且可以多次 await 同一个 Task 
 */

internal class Program
{
    static async Task Main(string[] args)
    {
        var setter = Task.Run(async () =>
        {
            Console.WriteLine("Setter is waiting for 2 seconds...");
            await Task.Delay(2000);
            Console.WriteLine("Setter is setting the result.");
        });

        var waiter = Task.Run(async () =>
        {
            Console.WriteLine("Waiter1 is waiting for the result...");
            //直接等待 setter 即可
            await setter;
            Console.WriteLine("Waiter1 received the result!");
        });

        await Task.WhenAll(setter, waiter);
    }
}

/*
 * 结语：在异步编程中使用同步机制更多是思想转变，在传统多线程中，Thread 之间没有关联，也不太能在一个 Thread 中观测另一个 Thread 的状态，因此需要各种锁来保护共享资源，控制线程的执行顺序等。
 *      而在异步编程中，可以通过 await 来等待另一个 Task 的完成，不需要显式的锁来控制并发。
 * 
 * 总结：
 *      1. await Task ：Task 本身即可 await 来控制并发，源生。
 *      2. SemaphoreSlim ：可以具体设置并发数量，源生。
 *      3. TaskCompletionSource : 一次性锁，且可以携带返回值，源生。
 *      4. AsyncAutoResetEvent ：能够自动 reset 的锁，需要导入第三方包。
 *      5. AsyncLock ：能够在异步方法中使用 lock 语句（这个锁可以同时用在同步和异步方法中），需要导入第三方包。
 */
