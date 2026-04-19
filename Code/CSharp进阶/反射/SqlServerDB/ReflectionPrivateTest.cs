using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDB
{
    /// <summary>
    /// 反射测试类，主要测试私有方法构建
    /// </summary>
    public class ReflectionPrivateTest
    {
        private int count;
        private char gender;
        public string Name { get; set; }
        public string Phone { get; set; }

        private ReflectionPrivateTest()
        {
            Console.WriteLine("私有无参构造执行...");
        }

        private ReflectionPrivateTest(string name)
        {
            Console.WriteLine("私有有参构造执行...参数是：" + name);
        }

        private void PrivateShow()
        {
            Console.WriteLine("我是 private 的 Show 方法");
        }

        public void PublicShow()
        {
            Console.WriteLine("我是 public 的 Show 方法");
        }

        private void TShow1<T>()
        {
            Console.WriteLine("我是 private 无参泛型的 Show 方法");
        }

        private void TShow2<T>(string name)
        {
            Console.WriteLine("我是 private 有参泛型的 TShow2 方法，参数是 name: " + name);
        }

        //用于解析重载问题的演示, 同名方法，不同参数
        private void TShow3<T>(int id, string name)
        {
            Console.WriteLine("我是 private 有参泛型的 TShow3 方法，用来演示解析重载问题，参数是：int:" + id + " name: " + name);
        }
        private void TShow3<T>(object obj, string name)
        {
            Console.WriteLine("我是 private 有参泛型的 TShow3 方法，用来演示解析重载问题，参数是：obj:" + obj.ToString() + " name: " + name);
        }
    }
}
