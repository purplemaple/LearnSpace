using Microsoft.Extensions.Configuration;

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile(
    "config.json",      //文件路径
    false,           //该文件是否可选(false: 找不到该文件就报错)
    true);      //文件修改时重新加载

IConfigurationRoot configRoot = configBuilder.Build();

//注：不管哪种方法都不区分大小写
/*
 * 1. 非常原始的读取方法
 */
string name = configRoot["name"]!;
string age1 = configRoot["age1"]!;                //json 中配置的是 -1, 而不是 "-1", 但框架能自动转成 string 类型
int age2 = int.Parse(configRoot["age2"]!);      //因为 json 默认读出的结果都是 string，因此这里需要手动转成 int
string address = configRoot.GetSection("proxy:address").Value!;

Console.WriteLine($"name = {name}");
Console.WriteLine($"age1 = {age1}");
Console.WriteLine($"age2 = {age2}");
Console.WriteLine($"addr = {address}");

/*
 * 2. 对象映射读取方法
 */
Proxy? proxy = configRoot.GetSection("proxy").Get<Proxy>();

Console.WriteLine($"address: {proxy.Address}; port: {proxy.Port}");




class Proxy
{
    public string Address { get; set; }

    public int Port { get; set; }
}