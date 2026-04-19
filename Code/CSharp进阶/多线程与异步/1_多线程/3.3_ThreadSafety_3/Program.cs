/*
 * 使用 Lazy 类可以直接实现单例模式，而不用像传统方式一样，外面套个锁，里面又套个锁
 * 
 * 注：Lazy 除了能简化单例模式外，它还能提供延迟加载的特性，因此用这种方法实现的是懒汉式单例模式
 */
public class Singleton
{
    private static readonly Lazy<Singleton> instance = new Lazy<Singleton>(() => new Singleton());

    private Singleton()
    {
        // 私有构造函数
    }

    public static Singleton Instance
    {
        get { return instance.Value; }
    }

    public void SomeMethod()
    {
        // 单例类的方法
    }
}