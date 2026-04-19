namespace _2_SemaphoreSlim;

/*
 * 2. SemaphoreSlim 是一个轻量级的信号量，允许在异步方法中自由设置具体可以有多少个线程并发访问
 */
internal class Program
{
    static async Task Main(string[] args)
    {
        var testDemo = new TestDemo();
        await testDemo.Run();
    }
}


class TestDemo
{
    SemaphoreSlim semaphoreSlim = new(3,3); //可以自由设置初始几个线程，最大几个线程同时进行（并非无限大，与CPU有关）

    DateTime start = DateTime.Now;

    List<Task<int>> tasks = [];

    public TestDemo()
    {
        // 创建多个异步任务来计算整数的平方，但这里使用异步锁确保同一时间只有一个异步任务执行
        tasks = Enumerable.Range(1, 10).Select(x => ComputeAsync(x, semaphoreSlim)).ToList();
    }


    /// <summary>
    /// 异步计算指定整数的平方，并在计算过程中使用异步锁确保线程安全。
    /// </summary>
    /// <remarks>This method acquires the specified asynchronous lock before performing the computation. Use
    /// this method when thread-safe access is required in asynchronous scenarios.</remarks>
    public async Task<int> ComputeAsync(int x, SemaphoreSlim semaphoreSlim)
    {
        //等待获取信号量
        await semaphoreSlim.WaitAsync();
        await Task.Delay(300); // 模拟一些异步工作
        semaphoreSlim.Release();
        return x * x;
    }

    public async Task Run()
    {
        var results = await Task.WhenAll(tasks);
        Console.WriteLine(string.Join(", ", results));

        var end = DateTime.Now;
        Console.WriteLine($"Elapsed: {(end - start).TotalMilliseconds:F4} ms");
    }
}

