namespace _2_FireAndForget;

//2. 使用 Fire and Forget 一发即忘，不阻塞地调用异步方法（可能导致无法捕获异常，或者程序崩溃）

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            //2.1 对于 async Task 标记的方法，外部无法捕获异常，主线程无报错
            _ = FooAsync(); //使用弃元写法避免警告

            //2.2 对于 async Void 标记的方法，程序直接崩溃
            VoidFooAsync();

            //主线程不会等待 FooAsync 方法，因此会直接输出 Hello, World! 后结束程序
            Console.WriteLine("Hello, World!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    static async Task FooAsync()
    {
        //await Task.Delay(1000);
        GetRandomResult();
        Console.WriteLine("FooAsync Done.");
    }

    static async void VoidFooAsync()
    {
        //await Task.Delay(1000);
        GetRandomResult();
        Console.WriteLine("FooAsync Done.");
    }

    //假装某一步会抛异常
    static void GetRandomResult() => throw new NotImplementedException();
}

