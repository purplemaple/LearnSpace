/*
 * 本项目用于演示多线程任务如何超时打断
 */
Thread thread = new(Foo);
thread.Start();

//当线程在两秒内完成则返回 true，否则返回 false
if (!thread.Join(TimeSpan.FromSeconds(2)))
{
    //超过两秒，打断线程
    thread.Interrupt();
}

void Foo()
{
    try
    {
        Console.WriteLine("Foo start...");
        Thread.Sleep(5000);
        Console.WriteLine("Foo end...");
    }catch(ThreadInterruptedException)
    {
        Console.WriteLine("Thread is Interrupted...");
    }
    
}