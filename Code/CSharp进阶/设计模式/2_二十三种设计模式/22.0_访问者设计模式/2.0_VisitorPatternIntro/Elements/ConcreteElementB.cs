using _2._0_VisitorPatternIntro.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_VisitorPatternIntro.Elements
{
    /// <summary>
    /// ConcreteElementB，具体元素类，实现 Accept 操作
    /// </summary>
    class ConcreteElementB : Element
    {
        public override void Accept(Visitor visitor)
        {
            visitor.VisitConcreteElementB(this);
        }
    }
}
