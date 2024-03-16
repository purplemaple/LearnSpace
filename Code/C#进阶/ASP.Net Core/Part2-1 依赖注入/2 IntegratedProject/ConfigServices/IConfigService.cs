using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigServices;

public interface IConfigService
{
    public string? GetValue(string name);
}

//从 Environment 中读取配置文件的实现类
public class EnvConfigService : IConfigService
{
    public string? GetValue(string name) => Environment.GetEnvironmentVariable(name);
}

//从 ini 文件中读取配置的实现类
public class IniFileConfigService : IConfigService
{
    //文件路径
    public string? FilePath { get; set; }

    public string? GetValue(string name)
    {
        var kv = File.ReadAllLines(FilePath)
                .Select( x => new { Name = x.Split('=')[0], Value = x.Split("=")[1] })
                .SingleOrDefault(kv => kv.Name == name);

        return kv?.Value;
    }
}
