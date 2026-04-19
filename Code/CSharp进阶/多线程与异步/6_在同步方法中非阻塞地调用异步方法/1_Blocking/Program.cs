namespace _1_Blocking;

//1. 使用 .Wait()或.Result() 方法以"阻塞"形式等待异步方法完成（这可能会造成死锁!）
internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Begin");

        //以阻塞形式等待异步方法完成可能会造成死锁！
        //死锁原因：当在 UI 线程或 ASP.NET 请求上下文中调用 .Wait() 或 .Result() 时，如果异步方法内部需要回到原始上下文（例如，使用 await 后续代码需要在 UI 线程上执行），但该上下文也正在阻塞等待异步方法，就会导致死锁。

        //FooAsync().Wait();
        string message = GetMessageAsync().Result;

        Console.WriteLine(message);
        Console.WriteLine("Done");

        //2. 使用 .GetAwaiter().GetResult() 方法，依旧阻塞，依旧有可能造成死锁，唯一区别：它会抛出原始异常而不是被包装成的 AggregateException。
        try
        {
            //FooAsync().Wait();
            FooAsync().GetAwaiter().GetResult();

            Console.WriteLine("Hello, World!");
            throw new Exception();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    static async Task FooAsync()
    {
        await Task.Delay(1000);
        //Console.WriteLine("FooAsync completed.");
        throw new NotImplementedException();
    }

    static async Task<string> GetMessageAsync()
    {
        await Task.Delay(1000);
        return "Hello, World!";
    }
}

