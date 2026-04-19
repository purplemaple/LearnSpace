using LogServices;
using Microsoft.Extensions.DependencyInjection;

//注：这里将命名空间修改为依赖注入包的空间，相当于直接给依赖注入包添加扩展方法(目的是为了减少 using 引用)
namespace Microsoft.Extensions.DependencyInjection;

public static class ConsoleLogExtensions
{
    //2.2 自定义扩展方法，调用这个方法可以直接以 ConsoleLogPrivider 这个实现类来提供服务
    public static void AddConsoleLog(this IServiceCollection service)
    {
        service.AddScoped<ILogPrivider, ConsoleLogPrivider>();
    }
}

