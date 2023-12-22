/*
 * 本项目用于演示多线程环境下的同步机制
 * 为了达到同步效果，使用了 BlockingCollection 集合 (ConcurrentQueue 实现)
 * 然而，在异步环境中不能使用这些方法，因为异步要求不能阻塞
 */
using System.Collections.Concurrent;

//消息队列，使用 ConcurrentQueue 实现原子化
var queue = new BlockingCollection<Message>(new ConcurrentQueue<Message>());

//生产者
var sender = new Thread(SendMessageThread);
//消费者
var receiver = new Thread(ReceiveMessageThread);

sender.Start(0);
receiver.Start(1);

sender.Join();

// make sure all messages are received
Thread.Sleep(100);

receiver.Interrupt();
receiver.Join();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();


//发送消息的任务
void SendMessageThread(object? arg)
{
    int id = (int)arg!;

    for (int i = 1; i<= 20; i++)
    {
        queue.Add(new Message(id, i.ToString()));
        Console.WriteLine("Thread " + id + " sent " + i);
        Thread.Sleep(100);
    }
}

//接收信息的任务
void ReceiveMessageThread(object? id)
{
    try
    {
        while (true)
        {
            var message = queue.Take();
            Console.WriteLine("Thread " + id + " reveived " + message.Content + " from " + message.FromId);
            Thread.Sleep(1);
        }
    }
    catch(ThreadInterruptedException)
    {
        Console.WriteLine("Thread " + id + "interrupted...");
    }
}



//record 关键字有什么用？
record Message(int FromId, string Content);