namespace _4_TaskCompletionSource;

/*
 * 4. TaskCompletionSource 内部维护了一个只读 Task 对象，外部通过 await 这个 Task 控制并发，而内部通过 SetResult、SetException 或 SetCanceled 方法来设置这个 Task 的状态。
 */
internal class Program
{
    static async Task Main(string[] args)
    {
        /*
         * TaskCompletionSource 内部维护了一个只读 Task 对象，外部通过 await 这个 Task 控制并发，而内部通过 SetResult、SetException 或 SetCanceled 方法来设置这个 Task 的状态。
         * 注：
         *      1. Task 不能多次完成，因此这是一次性异步锁。同时 TaskCompletionSource 也不能多次设置结果，否则会抛出 InvalidOperationException 异常。
         *      2. 使用 TrySetResult、TrySetException 和 TrySetCanceled 方法，避免多次设置结果时抛出异常。
         *      3. TaskCompletionSource 可以携带返回值
         * 
         */
        var tcs = new TaskCompletionSource();
        var tcsStr = new TaskCompletionSource<string>();

        var setter = Task.Run(async () =>
        {
            Console.WriteLine("Setter is waiting for 2 seconds...");
            await Task.Delay(2000);
            Console.WriteLine("Setter is setting the result.");
            tcs.SetResult();
            tcsStr.SetResult("Mission Completed!");     //携带返回值

            //防止多次设置结果导致 InvalidOperationException 异常
            if (!tcs.TrySetResult())
            {
                Console.WriteLine("tcs is Already set!");
            }
        });

        var waiter = Task.Run(async () =>
        {
            Console.WriteLine("Waiter is waiting for the result...");
            await tcs.Task;
            Console.WriteLine("Waiter received the result!");
            string res = await tcsStr.Task;             //等待并获取返回值
            Console.WriteLine(res);
        });

        await Task.WhenAll(setter, waiter);
    }
}
