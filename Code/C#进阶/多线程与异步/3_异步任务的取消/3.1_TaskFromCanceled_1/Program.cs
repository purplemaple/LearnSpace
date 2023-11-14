
CancellationToken token = default;
Demo demo = new();
Task<string> task = demo.FooAsync(token);

//使用 Task.IsCanceled 即可判断这个异步任务是否被取消
if(task.IsCanceled)
{
    //do something...
}

class Demo
{
    public Task<string> FooAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            //对于有返回值的 Task，被取消时采用 FromCanceled< > 返回
            return Task.FromCanceled<string>(cancellationToken);

        //对于有返回值的 Task，任务正常完成时采用 FromResult< > 返回
        return Task.FromResult("任务正常结束");
    }
}
