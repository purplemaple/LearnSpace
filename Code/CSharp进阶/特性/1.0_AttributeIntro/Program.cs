using LINQPad.FSharpExtensions;
using System.ComponentModel;
using System.Reflection;

namespace _1._0_AttributeIntro
{
    /// <summary>
    /// 特性 Attribute
    /// 前置课程: 泛型、反射、LINQ
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Student student = new()
            {
                Id = 13,
                Name = "Tom",
                Age = 17,
                Gender = Gender.Male,
                Class = "3A"
            };


            Serialize(student).Dump();
            Console.WriteLine(Serialize(student));

            string Serialize(object obj)
            {
                var res = obj       //要注意：这里不能用 object 接收，因为结果是 IEnumerable<string> 类型对象(该对象重写了ToString()方法)，因此打印时可以得到它的内容，
                    .GetType()                                   //而 object 类型对象没有实现 ToString() 方法，所以打印得到的是类名，而非内容
                    .GetProperties()
                    .Where(pi =>
                    {
                        BrowsableAttribute? attr = pi.GetCustomAttribute<BrowsableAttribute>();
                        if(attr != null)
                        {
                            return attr.Browsable;
                        }
                        return true;
                    })
                    .Select(pi => new { Key = pi.Name, Value = pi.GetValue(obj) })
                    .Select(o => $"{o.Key}: {o.Value}");

                return string.Join(Environment.NewLine, res);
            }
        }
    }

    class Student
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Class { get; set; }
    }

    enum Gender
    {
        Male,

        Female
    }
}