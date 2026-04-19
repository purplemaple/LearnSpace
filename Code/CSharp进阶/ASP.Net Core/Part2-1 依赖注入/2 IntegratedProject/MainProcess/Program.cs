ServiceCollection services = new ServiceCollection();

//1.1 使用 EnvConfigService 实现类，从环境中读取配置
//services.AddScoped<IConfigService, EnvConfigService>();

//1.2 使用 IniFileConfigService 实现类，从 ini 文件中读取配置
/*
 * 注：为什么这里必须使用 Lambda 表达式传入实现类对象，而不能直接 new 一个实现类对象传入？
 * 原因： AddScoped 是在运行时才创建对象，因此这里不能静态地 new 一个实现类传入，而应该用回调函数，运行时动态地 new 实现类对象后传入
 */
//services.AddScoped(typeof(IConfigService), _ => new IniFileConfigService() { FilePath = "mail.ini" });

/* 
 * 1.3 使用扩展方法
 * 注： 这两种服务的注册顺序会影响结果，根据 ConfigReader 中的逻辑，后注册者会覆盖前者
 */
services.AddIniFileConfig("mail.ini");
services.AddEnvFileConfig();


//2.1 原始方法，业务层仍需知道服务层的具体实现类的名称
//services.AddScoped<ILogPrivider, ConsoleLogPrivider>();

/*
 * 2.2 扩展方法，给 DependencyInjection 包下的 IServiceCollection 接口添加扩展方法，使得业务层可以 . 出对应的实现类服务
 * 补充：使用这种方法，可以将原接口的访问修饰符改回 internal，而不需要 public，从而将访问级别限制在其程序集内
 */
services.AddConsoleLog();


services.AddScoped<IMailService, MailService>();


/* 
 * 3.1 使用模拟集群环境时从不同地方读取多个配置文件的情况
 * 注：
 *      1. 把 1.1 services.AddScoped<IConfigService, EnvConfigService>() 这句代码放开，则现在可以从环境变量和 ini 配置文件两个地方读取配置
 *      2. 右键本项目 -> 属性 -> 调试 -> 打开调试启动配置文件UI -> 环境变量，添加 SmtpServer = HrxHrx.com
 *      3. 运行项目，此时项目会先后读取环境变量和 ini 文件中的 SmtpServer 属性配置，并保留最后一个不为空的配置
 */
services.AddLayeredConfig();

using (ServiceProvider sp = services.BuildServiceProvider())
{
    //第一个根服务必须通过 ServiceLocator(服务定位器) 的方式注册，其引用的其他服务则可以自动注册 (后续学习可以省略这步)
    IMailService mailService = sp.GetRequiredService<IMailService>();
    mailService.Send("Hello", "34567@gov.coom", "What's up?");
}

