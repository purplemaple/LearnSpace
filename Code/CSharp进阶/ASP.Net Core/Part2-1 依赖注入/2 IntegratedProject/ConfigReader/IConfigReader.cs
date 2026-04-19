// 3.1 本项目用于模拟集群环境时从不同地方读取多个配置文件的情况
using ConfigServices;

namespace ConfigReader;

public interface IConfigReader
{
    /// <summary>
    /// 如果配置找不到，就返回 null
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string? GetValue(string name);
}

internal class LayeredConfigReader : IConfigReader
{
    //用于装载所有提供配置服务的服务集合
    private readonly IEnumerable<IConfigService> services;

    public LayeredConfigReader(IEnumerable<IConfigService> services)
    {
        this.services = services;
    }

    public string? GetValue(string name)
    {
        string? value = null;
        foreach (var service in services)
        {
            string? newValue = service.GetValue(name);
            //逐层覆盖，取到最后一个不为 null 的值
            if (newValue != null)
            {
                value = newValue;
            }
        }

        return value;
    }
}
