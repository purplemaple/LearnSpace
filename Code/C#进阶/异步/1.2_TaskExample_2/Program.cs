async Task Main()
{
    //这里其实也用到了异步 Main 方法，Main 方法没被 async 标记的话，也不能使用 await
    //FooAsync 方法的返回类型是 Task，是异步操作，因此就可以 await 等待
    await FooAsync();
}

/*Task FooAsync()
{
    return Task.Delay(1000);
}*/

async Task FooAsync()
{
    //FooAsync 被 async 标记后，即可在其内部使用 await
    await Task.CompletedTask;  
}