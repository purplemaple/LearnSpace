using System.Diagnostics;

//生成一个 1 ~ 20 的数组
int[] inputs = Enumerable.Range(1, 20).ToArray();
var sw = Stopwatch.StartNew();

/* 信号量
 * Semaphore(int initialCount, int maximumCount)
 * initialCount：初始许可数量(信号量)
 * maximumCount：最大许可数量(信号量)
 * 
 * Semaphore 可以控制多个线程对共享资源的并发访问。
 * 其内部维护一个表示许可数量(信号量)的整数，initialCount 设置它的初始值，maximumCount 设置它的最大值。
 * 当一个线程调用WaitOne()方法时，它会尝试获取一个许可，如果成功，内部整数减一；
 * 当一个线程调用Release()方法时，它会释放一个许可，内部整数加一。
 * 如果内部整数为零，那么没有可用的许可，WaitOne()方法会阻塞当前线程，直到有其他线程释放许可。
 * 
 * new Semaphore(3, 3)：初始只有三个且最多只能有三个线程在干活
 * new Semaphore(3, 5)：初始只有三个，但最多可以有五个线程在干活(需要手动 Release() 两下，以将信号量增加到5个，否则它只会按3个运行，不会自己增加)
 */
Semaphore semaphore = new Semaphore(3, 5);

semaphore.Release();
semaphore.Release();
var outputs = inputs.AsParallel().AsOrdered().Select(HeavyJob).ToArray();


Console.WriteLine(string.Join(", ", outputs));
Console.WriteLine("Elapsed time：" + sw.ElapsedMilliseconds + " ms");

//使用完 Semaphore 后记得 Dispose() 以释放所有资源
semaphore.Dispose();


//计算平方值的函数
int HeavyJob(int input)
{
    //等待一个 Semaphore 许用信号量(阻塞等待，如果没等待就一直阻塞着等)
    semaphore.WaitOne();
    Thread.Sleep(100);
    //释放 Semaphore 许用信号量(如果不释放的话，信号量被抢完后，其他线程也不能进入了)
    semaphore.Release();
    return input * input;
}

