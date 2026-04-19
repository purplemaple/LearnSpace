using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_2_ServiceMethodOverload.Service
{
    internal class TestServiceImpl2 : ITestService
    {
        public string Name { get; set; }

        public void SayHi()
        {
            Console.WriteLine("你好，我是 " + Name);
        }
    }
}
