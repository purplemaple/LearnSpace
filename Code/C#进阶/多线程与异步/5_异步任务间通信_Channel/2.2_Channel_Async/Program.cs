//2. 使用 Channel 替换 BlockingCollection 同时改用异步写法

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


//record 关键字有什么用？
record Message(int FromId, string Content);