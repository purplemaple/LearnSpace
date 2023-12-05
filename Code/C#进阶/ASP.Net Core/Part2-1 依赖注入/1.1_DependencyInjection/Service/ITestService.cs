using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _1._1_DependenceInjection.Service
{
    internal interface ITestService
    {
        public string Name { get; set; }

        public void SayHi();
    }
}
