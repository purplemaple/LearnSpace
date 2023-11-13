class Demo
{
    async Task FooAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(5000);
        Console.WriteLine("hello, world");
    }

    /*
     * 方法一：
     * 使用无参重载，内部调用有参方法时，传入一个空 CancellationToken
     */
    async Task FooAsync()
    {
        await FooAsync(CancellationToken.None);
    }

    /*
     * 与上面的没区别，只是因为任务简单所以省略了 async 和 await 关键字
     */
    /*Task FooAsync()
    {
        return FooAsync(CancellationToken.None);
    }*/

    /*
     * 方法二：第1种
     * 使用默认参数，CancellationToken 是个结构体，其默认值就是 CancellationToken.None
     */
    async Task FooAsync2(int delay, CancellationToken cancellationToken = default)
    {
        await Task.Delay(delay);
    }

    /*
     * 方法二：第2种
     * 使用可空(可选)参数，使用方法时可传可不传，这里设定不传则为 null，然后在方法体内判空
     */
    async Task FooAsync2(int delay, CancellationToken? cancellationToken = null)
    {
        CancellationToken token = cancellationToken ?? CancellationToken.None;
        await Task.Delay(delay);
    }
}