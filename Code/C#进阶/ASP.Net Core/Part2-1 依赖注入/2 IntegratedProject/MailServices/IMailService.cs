using ConfigServices;
using LogServices;
using ConfigReader;

namespace MailServices;

public interface IMailService
{
    public void Send(string title, string targrt, string body);
}

public class MailService : IMailService
{
    //发邮件的服务中也需要记录日志以及读取配置，因此也注入日志服务
    private readonly ILogPrivider log;

    //1~2 原始模式，只读取单一环境下的配置文件，因此 IConfigService 即够用
    /*private readonly IConfigService config;

    public MailService(ILogPrivider log, IConfigService config)
    {
        this.log = log;
        this.config = config;
    }*/
    /*
     * 3.1 模拟集群环境时从不同地方读取多个配置文件的情况
     * 这里不再直接注入 IConfigService，而是利用 IConfigReader 来读取配置服务(该服务对象已实现读取全部配置文件的功能)
     */
    private readonly IConfigReader config;

    public MailService(ILogPrivider log, IConfigReader config)
    {
        this.log = log;
        this.config = config;
    }

    public void Send(string title, string targrt, string body)
    {
        this.log.LogInfo("准备发送邮件...");
        //模拟读取配置文件
        string? smtpServer = this.config.GetValue("SmtpServer");
        string? userName = this.config.GetValue("UserName");
        string? password = this.config.GetValue("Password");
        Console.WriteLine($"邮件服务器地址: {smtpServer} {userName} {password}");
        Console.WriteLine("发送邮件: " + title + " " + body);

        this.log.LogInfo("邮件发送完毕!");
    }
}

