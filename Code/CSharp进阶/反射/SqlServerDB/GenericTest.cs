using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDB
{
    /// <summary>
    /// 泛型类
    /// </summary>
    internal class GenericTest<T,W>
    {
        public GenericTest()
        {
            Console.WriteLine("泛型类的无参构造执行...类的泛型为：" + typeof(T) + " 和：" + typeof(W));
        }
        private void GenericMethod<TType>()
        {
            Console.WriteLine("1个泛型参数，无方法参数的方法执行...");
        }

        private void GenericMethod<TType>(int id, string name)
        {
            Console.WriteLine("1个泛型参数，2个方法参数的方法执行...\t 参数为：int：" + id + "\t string：" + name);
        }
    }
}
