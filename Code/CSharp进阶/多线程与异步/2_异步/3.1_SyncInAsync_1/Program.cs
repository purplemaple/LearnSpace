/*
 * 本例用于演示在异步编程中的同步操作
 * 即有时需要限制同时可运行的线程数量
 *      1. 如爬虫反封，每次只开启少数线程，
 *      2. 如考虑线程安全问题，只允许单线程操作共享资源
 */

int[] inputs = Enumerable.Range(1, 10).ToArray();

//设置初始线程为2，且最大线程数为2
SemaphoreSlim sem = new(2, 2);

//给每个 Task 添加 HeavyJob 任务
List<Task<int>> tasks = inputs.Select(HeavyJob).ToList();

await Task.WhenAll(tasks);
var outputs = tasks.Select(x => x.Result).ToArray();

//打印结果
foreach (var output in outputs)
{
    Console.WriteLine(output);
}

async Task<int> HeavyJob(int input)
{
    //等待信号量，因为创建 SemaphoreSlim 时设置了信号量上限为2，因此只有两个线程能抢到信号量并开始干活
    await sem.WaitAsync();

    await Task.Delay(1000);

    //释放信号量，释放完后其他线程才能获取信号量从而开始干活
    sem.Release();

    return input * input;
}