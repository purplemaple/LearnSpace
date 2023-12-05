//用于装载服务的集合
ServiceCollection services = new ServiceCollection();

/* 1. 以瞬态 (Transient) 的生命周期形式向集合中注册服务
 * 每次都创建一个新的对象
 */
//services.AddTransient<TestServiceImpl1>();
//构建服务定位器(ServiceProvider 对象)，注意，ServiceProvider 类实现了 IDisposable 接口，因此需要释放
/*using (ServiceProvider sp = services.BuildServiceProvider())
{
    //获取服务对象
    TestServiceImpl1 impl1 = sp.GetService<TestServiceImpl1>();

    impl1.Name = "Lily";
    impl1.SayHi();

    TestServiceImpl1 impl2 = sp.GetService<TestServiceImpl1>();
    impl2.Name = "Tom";
    impl2.SayHi();

    Console.WriteLine("注册瞬态服务，先后注册的两个服务对象引用是否相同：" + object.ReferenceEquals(impl1, impl2));
}*/


/* 2. 以单例 (Singleton) 的生命周期形式向集合中注册服务
 * 不管创建多少次，都是同一个对象
 */
/*services.AddSingleton<TestServiceImpl1>();
using (ServiceProvider sp = services.BuildServiceProvider())
{
    //获取服务对象
    TestServiceImpl1 impl3 = sp.GetService<TestServiceImpl1>();

    impl3.Name = "Lily";
    impl3.SayHi();

    TestServiceImpl1 impl4 = sp.GetService<TestServiceImpl1>();
    impl4.Name = "Tom";
    impl4.SayHi();

    Console.WriteLine("注册单例服务时，先后注册的两个服务对象引用是否相同：" + object.ReferenceEquals(impl3, impl4));
}*/


/* 3. 以范围 (Scope) 的生命周期形式向集合中注册服务
 * 在范围内，创建的都是同一个对象
 * 在范围外，创建的是不同对象
 */
services.AddScoped<TestServiceImpl1>();
using (ServiceProvider sp = services.BuildServiceProvider())
{
    //这样写只是为了提升作用域，为了后面判断引用是否相同，一般都是用的时候再声明
    TestServiceImpl1 impl5;
    TestServiceImpl1 impl6;
    TestServiceImpl1 impl7;

    //与 Transient 和 Singleton 不同的是，还要额外声明一个使用范围(IServiceScope)，在范围内服务对象引用相同，范围外服务对象引用不同
    using (IServiceScope scope1 = sp.CreateScope())
    {
        impl5 = scope1.ServiceProvider.GetService<TestServiceImpl1>();
        impl5.Name = "Jerry";
        impl5.SayHi();

        impl6 = scope1.ServiceProvider.GetService<TestServiceImpl1>();
        impl6.Name = "Jim";
        impl6.SayHi();
    }

    using (IServiceScope scope2 = sp.CreateScope())
    {
        impl7 = scope2.ServiceProvider.GetService<TestServiceImpl1>();
        impl7.Name = "Jerry";
        impl7.SayHi();
    }
    Console.WriteLine("注册范围服务时，同范围内先后注册的两个服务对象引用是否相同：" + object.ReferenceEquals(impl5, impl6));
    Console.WriteLine("注册范围服务时，不同范围内注册的两个服务对象引用是否相同：" + object.ReferenceEquals(impl5, impl7));
}


