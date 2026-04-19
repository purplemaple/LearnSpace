namespace _5_AsyncFactory;

//5. 使用异步工厂解决需要在类的构造方法中调用异步的问题
//缺点：
//  1. 没办法注册给 IOC 容器
//  2. 不方便实现单例  

/*
 * 解决方案：
 *      1. 注册 IOC 容器: Microsoft.Extensions.DependencyInjection
 *      2. 实现单例: AsyncLazy: Nito.AsyncEx, Microsoft.VisualStudio.Threading
 *      3. Task工厂: JoinableTaskFactory: Microsoft.VisualStudio.Threading
 *      4. 单元测试: Microsoft.VisualStudio.TestTools.UnitTesting
 * 
*/

/* 
 * 结语：在同步方法中调用异步方法不如直接拥抱异步编程
 * 异步编程生态：
 *      1. async Task Main      : 入口异步
 *      2. async void           : 委托注册
 *      3. Asynclazy            : 异步单例
 *      4. IAsyncEnumerable     : 异步数据流
 *      5. IAsyncDisposable     : 异步释放资源
 *      6. AsyncRelayCommand    : 异步绑定
 */

internal class Program
{

    //注意：这里是异步入口函数
    //异步入口函数在编译时会自动创建普通的 Main 函数，然后在其中使用 .GetAwaiter().GetResult() 的方式调用异步任务
    static async Task Main(string[] args)
    {
        var myService = await MyService.CreateAsync();
        Console.WriteLine($"Service is Created: {myService.IsServiceCreated}");
    }
}

class MyService
{
    public bool IsServiceCreated { get; private set; } = false;

    //私有构造
    private MyService()
    {

    }

    async Task InitAsync()
    {
        await Task.Delay(1000);
    }

    //使用工厂模式，就可以在外层 await CreateAsync() 拿到已经创建好的类对象
    public static async Task<MyService> CreateAsync()
    {
        var service = new MyService();
        await service.InitAsync();
        
        //throw new Exception();
        service.IsServiceCreated = true;
        return service;
    }
}