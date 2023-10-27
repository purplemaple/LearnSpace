using System.Collections.Concurrent;

/*
 * 本例基于项目 1.2，改用 ConcurrentQueue 来简便地实现生产者消费者模式
 */

//将普通队列(Queue)改为ConcurrentQueue即可不用加锁而做到线程安全
ConcurrentQueue<int> queue = new();

//生产者 -- 每隔 20 ms 往队列里添加一个数字
Thread producer = new(AddNumbers);

//消费者1、2，抢队列里的数字
Thread consumer1 = new(ReadNumbers);
Thread consumer2 = new(ReadNumbers);


producer.Start();
consumer1.Start();
consumer2.Start();

producer.Join();
consumer1.Join();
consumer2.Join();

void AddNumbers()
{
    for (int i = 0; i < 20; i++)
    {
        Thread.Sleep(20);
        queue.Enqueue(i);
    }
}

void ReadNumbers()
{
    try
    {
        while (true)
        {
            /*lock(lockObj)
            {
                if (queue.TryDequeue(out int res))
                {
                    Console.WriteLine(res);
                }
            }*/
            //不再需要加锁
            if (queue.TryDequeue(out int res))
            {
                Console.WriteLine(res);
            }
            Thread.Sleep(1);
        }
    }
    catch (ThreadInterruptedException)
    {
        Console.WriteLine("Thread interrupted.");
    }
}