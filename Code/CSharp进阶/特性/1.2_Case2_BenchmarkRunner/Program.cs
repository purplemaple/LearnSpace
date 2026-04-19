using System.Diagnostics;
using System.Reflection;

/*
 * 1. 使用泛型方式传参演示：
 */
/*BenchmarkRunner<SimpleTester>();

void BenchmarkRunner<T>() where T : new()       //1. 申明泛型是可实例化的
{
    T obj = new T();                            //2. 实例化泛型，后面要用到

    var methods = typeof(T)       //拿到传入泛型的 Type ( typeof() 的返回值是 Type 类型)
        .GetMethods()
        .Where(mi => mi.GetCustomAttribute<BenchmarkAttribute>() is not null);   //筛选，拿到标有 Benchmark 特性的方法
        
    foreach(var method in methods)
    {
        var sw = Stopwatch.StartNew();

        for (int i = 0; i <= 100_00_000; i++)
        {
            method.Invoke(obj, null);           //3. 调用方法，需要传入一个对象，指明这个方法是由哪个对象调用的  (null 是表示方法的参数)
        }

        Console.WriteLine(method.Name + " 耗时：" + sw.ElapsedMilliseconds);
    }
}*/

/*
 * 2. 使用参数列表方式传参演示：
 */
BenchmarkRunner(typeof(SimpleTester));

void BenchmarkRunner(Type type)
{
    object? obj = Activator.CreateInstance(type);   //2. 实例化对象，后面要用到

    var methods = type
        .GetMethods()
        .Where(mi => mi.GetCustomAttribute<BenchmarkAttribute>() is not null);   //筛选，拿到标有 Benchmark 特性的方法

    foreach (var method in methods)
    {
        var sw = Stopwatch.StartNew();

        for (int i = 0; i <= 100_00_000; i++)
        {
            method.Invoke(obj, null);           //3. 调用方法，需要传入一个对象，指明这个方法是由哪个对象调用的  (null 是表示方法的参数)
        }

        Console.WriteLine(method.Name + " 耗时：" + sw.ElapsedMilliseconds);
    }
}
public class SimpleTester
{
    private IEnumerable<int> testList = Enumerable.Range(1, 10).ToArray();

    [Benchmark]
    public int CalcMinByLINQ()
    {
        return testList.Min();
    }

    [Benchmark]
    public int CalcMinNaive()
    {
        int min = int.MaxValue;
        foreach (int i in testList)
        {
            if(i < min) min = i;
        }
        return min;
    }
}

[AttributeUsage(AttributeTargets.Method)]
class BenchmarkAttribute : Attribute { }