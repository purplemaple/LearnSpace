using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_TemplatePatternIntro.Templates
{
    /// <summary>
    /// 抽象的模板类，定义并实现了一个模板方法，这个模板方法一般是一个具体方法(注意既不是抽象方法也不是虚方法)，
    /// 该具体方法给出了一个顶级逻辑的骨架，而这个逻辑骨架的组成又是一系列相应的抽象操作，这些抽象操作推迟到子类中实现
    /// </summary>
    abstract class AbstractClass
    {
        public abstract void PrimitiveOpration1();
        public abstract void PrimitiveOpration2();

        //注意本方法是具体方法，而内部才是一系列抽象方法，需要交由子类实现
        public void TemplateMethod()
        {
            PrimitiveOpration1();
            PrimitiveOpration2();
            Console.WriteLine("");
        }
    }
}
