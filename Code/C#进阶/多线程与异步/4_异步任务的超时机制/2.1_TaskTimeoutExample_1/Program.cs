/*
 * 本项目用于演示异步任务如何超时打断
 * 方法一：Task.WhenAny()
 *      1. 使用 Task.WhenAny() 传入两个任务，第一个任务是原异步任务，第二个是等待任务(设定几秒超时就等待几秒)
 *      2. 因为 Task.WhenAny() 会返回最先等待到的那个任务，因此若拿到的不是原异步任务，则证明其已超时
 */

CancellationTokenSource cts = new();
Task fooTask = FooAsync(cts.Token);

/* 这里传入两个 Task，当任一个 Task 结束时就停止等待
 * fooTask 需要 5 秒完成，但 Task.Delay(2000) 只需要 2 秒就能完成
 * 因此这里返回的结果不是 fooTask
 */
Task completedTask = await Task.WhenAny(fooTask, Task.Delay(2000));
if(completedTask != fooTask)    //返回的不是 fooTask，证明它超时了
{
    //fooTask超时，取消掉
    cts.Cancel();
    await fooTask;  //如果没有这一句就不会输出 catch 里面的语句
    Console.WriteLine("Timeout...");
}

Console.WriteLine("Done...");

async Task FooAsync(CancellationToken token)
{
    try
    {
        Console.WriteLine("Foo start...");
        await Task.Delay(5000, token);
        Console.WriteLine("Foo end...");
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Foo is Canceled...");
    }
    finally
    {
        cts.Dispose();
    }
}
