
Helper.PrintThreadId("Before", "Main");
await FooAsync();
Helper.PrintThreadId("After", "Main");

async Task FooAsync()
{
    Helper.PrintThreadId("Before", "FooAsync");
    await Task.Delay(1000);
    Helper.PrintThreadId("After", "FooAsync");

    /*int result = await GetValueAsync();
    int result = GetValueAsync().Result;

    Console.WriteLine(result);*/
}

/*async Task<int> GetValueAsync()
{
    await Task.Delay(1000);
    return 24;
}*/

class Helper
{
    //用于记录这是第几次输出
    private static int index = 1;

    public static void PrintThreadId(string? message = null, string? name = null)
    {
        string title = index + ": " + name;
        if (!string.IsNullOrEmpty(message))
        {
            title += " @ " + message;
        }
        //输出当前线程的 Id
        Console.WriteLine(title + "  线程Id：" + Environment.CurrentManagedThreadId);
        Interlocked.Increment(ref index);
    }
}