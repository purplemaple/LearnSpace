using _2._0_VisitorPatternIntro.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_VisitorPatternIntro.Elements
{
    /// <summary>
    /// Element类，定义一个Accept操作，它以一个访问者为参数
    /// </summary>
    abstract class Element
    {
        public abstract void Accept(Visitor visitor);
    }
}
