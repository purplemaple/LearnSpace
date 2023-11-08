Thread t1 = new(() =>
{
    try
    {
        while (true)
        {
            //没有这句的话就永远打断不了
            Thread.Sleep(0);
        }
        /*for (int i = 0; i < 20; i++)
        {
            Thread.Sleep(200);
            Console.WriteLine("子线程：正在运行...");
        }*/
    }
    catch (ThreadInterruptedException)
    {
        //使用 Interrupt() 打断线程时，会抛出 ThreadInterruptedException 异常，这里捕获一下即可
        Console.WriteLine("子线程：线程被打断！");
    }
    finally
    {
        Console.WriteLine("子线程：运行完毕！");
    }
})
{ IsBackground = true, Priority = ThreadPriority.Normal };  //配置线程，是后台线程，优先级为 Normal

t1.Start();

//主线程等 1000 ms后打断子线程
Thread.Sleep(1000);
t1.Interrupt();

/*Console.WriteLine("主线程：正在等待子线程运行完毕");*/

t1.Join();
Console.WriteLine("主线程：结束！");