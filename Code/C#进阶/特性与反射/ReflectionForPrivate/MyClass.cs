using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionForPrivate
{
    internal class MyClass
    {
        private string Field = "我是私有字段";
        private static string staticField = "我是静态私有字段";

        private string myProperty { get; set; } = "我是私有属性";
/*
        private MyClass()
        {
            Console.WriteLine("私有构造函数执行...");
        }
*/
        private void FunA()
        {
            Console.WriteLine("私有方法执行..."); ;
        }

        private static void FunB()
        {
            Console.WriteLine("静态私有方法执行...");
        }
    }
}
