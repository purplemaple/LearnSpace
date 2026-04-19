using _2._0_VisitorPatternIntro.Elements;
using _2._0_VisitorPatternIntro.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_VisitorPatternIntro
{
    /// <summary>
    /// 对象结构类，能枚举它的元素，可以提供一个高层的接口以允许访问者访问它的元素
    /// </summary>
    class ObjectStructure
    {
        private IList<Element> elements = new List<Element>();

        public void Attach(Element element)
        {
            elements.Add(element);
        }

        public void Detach(Element element)
        {
            elements.Remove(element);
        }

        public void Accept(Visitor visitor)
        {
            foreach(Element e in elements)
            {
                e.Accept(visitor);
            }
        }
    }
}
