using System.Reflection;

namespace ReflectionForPrivate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new();

            Type type = obj.GetType();

            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;

            FieldInfo? fieldInfo = type.GetField("Field", flags);
            fieldInfo?.SetValue(obj, "若幸");
            Console.WriteLine(fieldInfo?.GetValue(obj));

            PropertyInfo? propertyInfo = type.GetProperty("myProperty", flags);
            Console.WriteLine(propertyInfo?.GetValue(obj));
            propertyInfo?.SetValue(obj, "新的私有属性值");
            Console.WriteLine(propertyInfo?.GetValue(obj));

            MethodInfo? method = type.GetMethod("FunA", flags);
            method?.Invoke(obj, null);

            Console.WriteLine("------------------------------------");

            flags = BindingFlags.Static | BindingFlags.NonPublic;
            fieldInfo = type.GetField("staticField", flags);
            Console.WriteLine(fieldInfo?.GetValue(null));

            method = type.GetMethod("FunB", flags);
            method?.Invoke(obj, null);
            method?.Invoke(null, null);     //静态方法是属于类的而不是属于对象的, 因此这里参数可以传null而不用传对象, 当然, 传对象也不会报错
        }
    }
}