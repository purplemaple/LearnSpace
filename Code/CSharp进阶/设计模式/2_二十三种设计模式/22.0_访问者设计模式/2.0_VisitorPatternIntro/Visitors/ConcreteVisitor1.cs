using _2._0_VisitorPatternIntro.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_VisitorPatternIntro.Visitors
{
    /// <summary>
    /// 具体访问者类，实现每个由Visitor声明的操作。每个操作实现算法的一部分，而该算法片段乃是对应于结构中对象的类
    /// </summary>
    class ConcreteVisitor1 : Visitor
    {
        public override void VisitConcreteElementA(ConcreteElementA concreteElementA)
        {
            Console.WriteLine("{0}被{1}访问", concreteElementA.GetType().Name, this.GetType().Name);
        }

        public override void VisitConcreteElementB(ConcreteElementB concreteElementB)
        {
            Console.WriteLine("{0}被{1}访问", concreteElementB.GetType().Name, this.GetType().Name);
        }
    }
}
