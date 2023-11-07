var inputs = Enumerable.Range(1, 10).ToArray();

//创建一个 Task 集合
List<Task<int>> tasks = new();

//遍历 Task 集合，给每个 Task 添加 HeavyJob 任务
foreach (var input in inputs)
{
    tasks.Add(HeavyJob(input));
}

/* 调用 .Result 时，如果任务没完成，则阻塞主线程，等待任务完成拿到结果后再继续
 * 这种阻塞方式不太好，因此在调用前加上一句 await Task.WhenAll()，即可异步地等待所有任务完成
 * 这样，再调用 .Result 时，因为所有任务都已完成，所以不会阻塞
 * 
 * 此外还有 Task.WhenAny()：任何一个任务完成就结束等待
 */
await Task.WhenAll(tasks);
var outputs = tasks.Select(x => x.Result).ToArray();

//打印结果
foreach(var output in outputs)
{
    Console.WriteLine(output);
}

async Task<int> HeavyJob(int input)
{
    //模拟耗时操作
    await Task.Delay(2000);
    return input * input;
}