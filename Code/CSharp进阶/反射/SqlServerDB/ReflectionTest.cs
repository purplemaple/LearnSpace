using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDB
{
    /// <summary>
    /// 反射测试类
    /// </summary>
    public class ReflectionTest
    {
        public ReflectionTest()
        {
            Console.WriteLine("这是无参构造");
        }

        public ReflectionTest(string name)
        {
            Console.WriteLine("这是有参构造，参数是：" + name);
        }
    }
}
