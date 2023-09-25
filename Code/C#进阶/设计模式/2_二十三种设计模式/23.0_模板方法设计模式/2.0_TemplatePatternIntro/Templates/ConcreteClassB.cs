using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_TemplatePatternIntro.Templates
{
    /// <summary>
    /// 具体类，实现抽象模板父类的抽象方法
    /// </summary>
    class ConcreteClassB : AbstractClass
    {
        public override void PrimitiveOpration1()
        {
            Console.WriteLine("具体类 B 实现方法 1");
        }
        public override void PrimitiveOpration2()
        {
            Console.WriteLine("具体类 B 实现方法 2");
        }
    }
}
