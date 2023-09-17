using System.Reflection;

namespace _2._0_GenericExample
{
    /// <summary>
    /// 演示创建泛型类的操作
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.LoadFrom("SqlServerDB.dll");
            /*
             * 注:
             *      泛型类在编译后生成的文件名会额外多出后缀：`n ，n = 泛型参数个数
             *      这里有两个泛型参数，所以 `2，MakeGenericType() 指定具体的泛型参数类型
             */
            //Type type = assembly.GetType("SqlServerDB.GenericTest`2").MakeGenericType(new Type[] { typeof(int), typeof(string) });
            Type type = assembly.GetType("SqlServerDB.GenericTest`2").MakeGenericType(typeof(int), typeof(string)); //简写

            /*
             * Type.Constructor() 法构建对象
             */
            ConstructorInfo? constructorInfo = type.GetConstructor(new Type[] { }); //即使是调用无参构造，这里也需要给一个空数组，不能给 null
            object obj1 = constructorInfo.Invoke(null);

            /*
             * Activator.CreateInstance() 法构建对象
             */
            object obj2 = Activator.CreateInstance(type, true);

            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            //调用：1个泛型，无参数的重载方法
            MethodInfo methodInfo1 = type.GetMethod("GenericMethod", flags, new Type[] { }).MakeGenericMethod(typeof(char));
            methodInfo1.Invoke(obj1, null);

            //调用：1个泛型，2个参数的重载方法
            MethodInfo methodInfo2 = type.GetMethod("GenericMethod", flags, new Type[] { typeof(int), typeof(string) }).MakeGenericMethod(typeof(char));
            methodInfo2.Invoke(obj1, new object[] {2, "若幸"});
        }
    }
}