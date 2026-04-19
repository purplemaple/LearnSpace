using _2._0_VisitorPatternIntro.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_VisitorPatternIntro.Visitors
{
    /// <summary>
    /// Visitor类，为该对象结构中 ConcreteElement 的每一个类声明一个 Visit 操作
    /// </summary>
    abstract class Visitor
    {
        public abstract void VisitConcreteElementA(ConcreteElementA concreteElementA);
        public abstract void VisitConcreteElementB(ConcreteElementB concreteElementB);
    }
}
