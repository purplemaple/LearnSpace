# 异步任务的通信_Channel
## 1 同步任务中的通信方式 BlockingCollection
- 本章节使用生产者-消费者模式来演示多线程(同步)环境下的同步机制
- 为了达到同步效果，使用了 BlockingCollection 集合 (ConcurrentQueue 实现)
- **然而，在异步环境中不能使用这些方法，因为异步要求不能阻塞**

```cs
using System.Collections.Concurrent;

//消息队列，使用 ConcurrentQueue 实现原子化
var queue = new BlockingCollection<Message>(new ConcurrentQueue<Message>());

//生产者
var sender = new Thread(SendMessageThread);
//消费者
var receiver1 = new Thread(ReceiveMessageThread);
var receiver2 = new Thread(ReceiveMessageThread);

sender.Start(0);
receiver1.Start(1);
receiver2.Start(2);

sender.Join();

// make sure all messages are received
Thread.Sleep(100);

receiver1.Interrupt();
receiver2.Interrupt();
receiver1.Join();
receiver2.Join();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();


//发送消息的任务
void SendMessageThread(object? arg)
{
    int id = (int)arg!;

    for (int i = 1; i <= 20; i++)
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
    catch (ThreadInterruptedException)
    {
        Console.WriteLine("Thread " + id + " interrupted...");
    }
}



//record 关键字有什么用？
record Message(int FromId, string Content);
```

## 2 使用 Channel 解决异步任务的通信问题

### 2.1 使用 Channel 但仍然是同步写法
- 在 [第 1 章](#1-同步任务中的同步方式-blockingcollection) 的基础上，使用 Channel
 替换 BlockingCollection, 但其它不变，仍采用同步写法

- **注: 为避免繁杂的初始化过程, Channel 只提供以静态方法的方式创建，而不能直接使用 new() 创建**

```cs
//1. 只用 Channel 替换 BlockingCollection 而依然采用同步写法

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
BoundedChannelOptions BoundedOption = new(10)
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
UnboundedChannelOptions UnboundedOptions = new()
{
    //比含边界的Channel少两个属性，其他都一样
    AllowSynchronousContinuations = true,
    SingleReader = false,
    SingleWriter = false,
};


/*
 * 注：Channel 的内部构造很复杂，如果直接 new 的话有很多东西不方便初始化，因此使用静态类来创建。这也类似于一种工厂模式
 */
var channel = Channel.CreateUnbounded<Message>(UnboundedOptions);



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

// 发送消息的任务
void SendMessageThread(object? arg)
{
    int id = (int)arg!;

    for (int i = 1; i <= 20; i++)
    {
        if (channel.Writer.TryWrite(new Message(id, i.ToString())))
            Console.WriteLine("Thread " + id + " sent " + i);
        Thread.Sleep(100);
    }
}

// 接收信息的任务
void ReceiveMessageThread(object? id)
{
    try
    {
        while (true)
        {
            if (channel.Reader.TryRead(out Message? message))
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
```

### 2.2 使用 Channel 的异步写法
- 在 [第 2.1 章](#21-使用-channel-但仍然是同步写法) 的基础上，改用异步方法

1. 不再使用线程中的 Start() 而是用异步中的 await
```cs
var channel = Channel.CreateUnbounded<Message>(UnboundedOptions);

using var cts = new CancellationTokenSource();

//生产者
Task senderTask = SendMessageAsync(channel.Writer, 1);
//消费者
Task receiverTask = ReceiveMessageAsync(channel.Reader, 2, cts.Token);

await senderTask;
//make sure all message are received
await Task.Delay(100);

cts.Cancel();

await receiverTask;

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
```

2. 调用方法改为 async, 任务结束时采用 CancellationToken 结束
```cs
// 发送消息的任务
async Task SendMessageAsync(ChannelWriter<Message> writer, int id)
{
    for (int i = 1; i <= 20; i++)
    {
        await writer.WriteAsync(new Message(id, i.ToString()));
        await Console.Out.WriteLineAsync("Thread " + id + " sent " + i);
        await Task.Delay(100);
    }
}

// 接收信息的任务
async Task ReceiveMessageAsync(ChannelReader<Message> reader, int id, CancellationToken token)
{
    try
    {
        while (!token.IsCancellationRequested)
        {
            Message message = await reader.ReadAsync(token);
            await Console.Out.WriteLineAsync("Thread " + id + " reveived " + message.Content + " from " + message.FromId);

        }
    }
    catch (OperationCanceledException)
    {
        await Console.Out.WriteLineAsync($"Task {id} cancelled...");
    }
}
```

### 2.3 Complete, 关闭 Channel 的源生方式
- 在 [第 2.2 章](#22-使用-channel-的异步写法) 的基础上，使用 Channel 源生提供的 Complete() 方式来关闭 Channel, 而非 CancellationToken
- **注：异常类型会从 OperationCanceledException 改为 ChannelClosedException**

1. 关闭异步任务，从 `cts.Cancel()`  修改为 `channel.Writer.Complete()`
```cs
/*
 * 注意：
 *      1. 这里是在 Channel 对象的 Writer 上调用
 *      2. 下面判断时是在 Channel 对象的 Reader 的属性上判断
 */
channel.Writer.Complete();
```

2. 判断任务是否结束，从 `while (!token.IsCancellationRequested)` 改为 `while (!reader.Completion.IsCompleted)`
```cs
// 接收信息的任务
async Task ReceiveMessageAsync(ChannelReader<Message> reader, int id)
{
    try
    {
        while (!reader.Completion.IsCompleted)
        {
            Message message = await reader.ReadAsync();
            await Console.Out.WriteLineAsync("Thread " + id + " reveived " + message.Content + " from " + message.FromId);

        }
    }
    catch (ChannelClosedException)
    {
        await Console.Out.WriteLineAsync($"Task {id} closed...");
    }
}
```

### 2.4 Complete 在多生产者、消费者环境下的使用
- 在 [第 2.3 章](#23-complete-关闭-channel-的源生方式) 的基础上，增加生产者和消费者的数量，检验是否能正常 Complete

```cs
var channel = Channel.CreateUnbounded<Message>(UnboundedOptions);

//生产者
Task senderTask0 = SendMessageAsync(channel.Writer, 0);
Task senderTask1 = SendMessageAsync(channel.Writer, 1);
//消费者
Task receiverTask2 = ReceiveMessageAsync(channel.Reader, 2);
Task receiverTask3 = ReceiveMessageAsync(channel.Reader, 3);

await Task.WhenAll(senderTask0, senderTask1);
//make sure all message are received
/*
 * 这里的 Delay 可以不需要了，因为当队列中还有消息没有被消费完时，即使调用了 channel.Writer.Complete()，也不会立刻关闭掉 channel
 * 原因：
 *      1. 关闭 channel 时调用的是 Writer 上的 Complete()，而消费时判断的是 Reader 上的 Completion.IsCompleted。
 *      2. 当调用 Writer.Complete() 且队列中没有任何消息时，Reader.Completion.IsCompleted 才会变成 true
 */
//await Task.Delay(100);

channel.Writer.Complete();

await receiverTask2;
await receiverTask3;

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

// 发送消息的任务
async Task SendMessageAsync(ChannelWriter<Message> writer, int id)
{
    for (int i = 1; i <= 20; i++)
    {
        await writer.WriteAsync(new Message(id, i.ToString()));
        await Console.Out.WriteLineAsync("Thread " + id + " sent " + i);
        await Task.Delay(100);
    }
}

// 接收信息的任务
async Task ReceiveMessageAsync(ChannelReader<Message> reader, int id)
{
    try
    {
        /* 注：
         * 1. 关闭 channel 时调用的是 Writer 上的 Complete()，而消费时判断的是 Reader 上的 Completion.IsCompleted。
         * 2. 当调用 Writer.Complete() 且队列中没有任何消息时，Reader.Completion.IsCompleted 才会变成 true
         */
        while (!reader.Completion.IsCompleted)
        {
            Message message = await reader.ReadAsync();
            await Console.Out.WriteLineAsync("Thread " + id + " reveived " + message.Content + " from " + message.FromId);

        }
    }
    catch (ChannelClosedException)
    {
        await Console.Out.WriteLineAsync($"Task {id} closed...");
    }
}
```

### 2.5 AwaitForeach, .Net 8.0 新语法
- 本章节使用 .Net 8.0 下的新语法 await foreach 来简化轮询接收的代码

```cs
// 接收信息的任务
async Task ReceiveMessageAsync(ChannelReader<Message> reader, int id)
{
    /*try
    {
        while (!reader.Completion.IsCompleted)
        {
            Message message = await reader.ReadAsync();
            await Console.Out.WriteLineAsync("Thread " + id + " reveived " + message.Content + " from " + message.FromId);

        }
    }
    catch (ChannelClosedException)
    {
        await Console.Out.WriteLineAsync($"Task {id} closed...");
    }*/

    //注：此语法只能在 .Net 8.0 之后使用
    await foreach(var message in reader.ReadAllAsync())
    {
        await Console.Out.WriteLineAsync("Thread " + id + " reveived " + message.Content + " from " + message.FromId);
        await Task.Delay(10);
    }
}
```