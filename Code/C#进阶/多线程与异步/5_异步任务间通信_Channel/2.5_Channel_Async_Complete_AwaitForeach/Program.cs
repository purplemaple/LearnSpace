// 5. 使用 .Net 8.0 的新语法 await foreach 来接收消息

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

await Task.WhenAll(receiverTask2, receiverTask3);

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
    /*try
    {
        *//* 注：
         * 1. 关闭 channel 时调用的是 Writer 上的 Complete()，而消费时判断的是 Reader 上的 Completion.IsCompleted。
         * 2. 当调用 Writer.Complete() 且队列中没有任何消息时，Reader.Completion.IsCompleted 才会变成 true
         *//*
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


//record 关键字有什么用？
record Message(int FromId, string Content);