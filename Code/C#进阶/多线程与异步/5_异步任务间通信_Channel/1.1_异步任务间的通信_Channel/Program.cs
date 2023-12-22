/*
 * 本项目用于演示多线程环境下的同步机制
 * 为了达到同步效果，使用了 BlockingCollection 集合 (ConcurrentQueue 实现)
 * 然而，在异步环境中不能使用这些方法，因为异步要求不能阻塞
 */
using System.Threading.Channels;


/*
 * 创建 Channel：
 *      1. 创建方式：
 *          Channel.Createbounded ：     创建一个有边界的 Channel
 *          Channel.CreateUnbounded ：   创建一个无边界的 Channel

 *      2. 参数：可以指定创建 Channel 时的一些参数
 *          BoundedChannelOptions:      可定义一些有边界 Channel 的参数
 *          UnboundedChannelOptions：   可定义一些无边界 Channel 的参数
 *      
 */
BoundedChannelOptions option = new(10)
{
    //Capacity = 10,                                    // Channel 的最大容量, new 的时候已经传入了，这里省略
    FullMode = BoundedChannelFullMode.Wait,             // 当 Channel 满时采取的策略 --> 等待
    //FullMode = BoundedChannelFullMode.DropWrite,      // 当 Channel 满时采取的策略 --> 把要写入的丢了
    //FullMode = BoundedChannelFullMode.DropNewest,     // 当 Channel 满时采取的策略 --> 把最新的丢了
    //FullMode = BoundedChannelFullMode.DropOldest,     // 当 Channel 满时采取的策略 --> 把最老的丢了
    SingleReader = true,                                // 是否只能有一个 Reader
    SingleWriter = true,                                // 是否只能有一个 Writer
    //AllowSynchronousContinuations = true,             // 是否允许以同步方式调用所有延续(continuation)   true: 可以增加性能，但可能导致死锁或堆栈溢出
};


/*
 * 注：Channel 的内部构造很复杂，如果直接 new 的话有很多东西不方便初始化，因此使用静态类来创建。这也类似于一种工厂模式
 */
var channel = Channel.CreateUnbounded<Message>();

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

    for (int i = 1; i <= 20; i++)
    {
        if(channel.Writer.TryWrite(new Message(id, i.ToString())))
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
            if(channel.Reader.TryRead(out Message? message))
                Console.WriteLine("Thread " + id + " reveived " + message.Content + " from " + message.FromId);
            Thread.Sleep(1);
        }
    }
    catch (ThreadInterruptedException)
    {
        Console.WriteLine("Thread " + id + "interrupted...");
    }
}



//record 关键字有什么用？
record Message(int FromId, string Content);