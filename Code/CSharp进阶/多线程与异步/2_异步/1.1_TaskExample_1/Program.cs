Task<string> task = new(() =>
{
    Thread.Sleep(1500);
    return "done.";
});

Console.WriteLine("1：" + task.Status);
task.Start();
//如果异步任务刚开始马上输出状态，则结果为 WaitingToRun
//哪怕只休眠 1 ms再输出，也会变成 Running，除非休眠 0 ms
//Thread.Sleep(0);
Console.WriteLine("2：" + task.Status);
Thread.Sleep(1000);
Console.WriteLine("3：" + task.Status);
Thread.Sleep(2000);
Console.WriteLine("4：" + task.Status);

Console.WriteLine("Result" + task.Result);