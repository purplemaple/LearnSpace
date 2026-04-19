//用于装载服务的集合
ServiceCollection service = new ServiceCollection();

//1. 注册服务时可以传入两种泛型，前者以接口提供服务，后者以实现类实现服务
//service.AddScoped<ITestService, TestServiceImpl1>();
//service.AddScoped(typeof(ITestService), typeof(TestServiceImpl1));

/*
 * 注意：这种在传入实现类时直接 new 的重载只有 AddSingleton 可以用，其他两种需要传入回调函数(详见)
 * 适用于：实现类需要初始化某些属性时，可以先初始化实现类，然后再传入使用
 */
service.AddSingleton(typeof (ITestService), new TestServiceImpl1());
//瞬态注册，传入回调函数示例：
//service.AddTransient(typeof(ITestService), _ => new TestServiceImpl1());

using (ServiceProvider sp = service.BuildServiceProvider())
{
    //2. 获取服务时直接获取接口(注册时已经声明使用哪种实现类，因此这里可以直接获取对应实现类对象)
    //2.1 GetService<T>() : 如果找不到服务则返回 null
    ITestService? ts1 = sp.GetService<ITestService>();

    //2.2 GetRequiredService<T>() : 如果找不到服务则直接报错
    ITestService ts2 = sp.GetRequiredService<ITestService>();

    //2.3 GetService() : 非泛型写法，因为涉及到类型转换，因此除反射等非必要情况一般不用
    ITestService? ts3 = (ITestService?)sp.GetService(typeof (ITestService));

    ts1!.Name = "Tom";
    ts1.SayHi();
    Console.WriteLine(ts1.GetType());


}

Console.WriteLine("--------------------------------------");
Console.WriteLine("--------------------------------------");


service.AddScoped<ITestService, TestServiceImpl2>();

using (ServiceProvider sp = service.BuildServiceProvider())
{
    //2.4 注册有多个服务时，可以通过 GetServices<T>() 获取所有服务组成的集合
    IEnumerable<ITestService> tss = sp.GetServices<ITestService>();
    foreach (ITestService ts in tss)
    {
        Console.WriteLine(ts.GetType());
    }

    Console.WriteLine("--------------------------------------");
    Console.WriteLine("--------------------------------------");

    //2.5 如果注册有多个服务，但只取一个，则得到最后注册的那个服务
    ITestService ts4 = sp.GetRequiredService<ITestService>();
    Console.WriteLine(ts4.GetType());
}