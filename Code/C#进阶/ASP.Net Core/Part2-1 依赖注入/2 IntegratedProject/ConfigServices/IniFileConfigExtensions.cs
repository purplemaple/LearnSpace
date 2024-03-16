using ConfigServices;
using Microsoft.Extensions.DependencyInjection;

//注：这里将命名空间修改为依赖注入包的空间，相当于直接给依赖注入包添加扩展方法(目的是为了减少 using 引用)
namespace Microsoft.Extensions.DependencyInjection
{
    public static class IniFileConfigExtensions
    {
        public static void AddIniFileConfig(this IServiceCollection services, string filePath)
        {
            services.AddScoped(typeof(IConfigService), _ => new IniFileConfigService { FilePath = filePath });
        }

        public static void AddEnvFileConfig(this IServiceCollection services)
        {
            services.AddScoped<IConfigService, EnvConfigService>();
        }
    }
}
