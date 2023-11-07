
Console.WriteLine("当前线程ID：" + Environment.CurrentManagedThreadId);
int res = await Task.Run(HeavyJob);
Console.WriteLine(res);

//HeavyJob 原本是一个同步任务，使用 Task.Run 包装后变成异步任务，因此任务内外的线程 Id 不同

/*
 * 模拟一个耗时较长的同步任务
 */
int HeavyJob()
{
    Thread.Sleep(3000);
    Console.WriteLine("当前线程ID：" + Environment.CurrentManagedThreadId);
    return 10;
}