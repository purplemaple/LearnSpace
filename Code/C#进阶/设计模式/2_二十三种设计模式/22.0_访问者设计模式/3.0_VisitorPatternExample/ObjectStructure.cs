using _3._0_VisitorPatternExample.Actions;
using _3._0_VisitorPatternExample.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_VisitorPatternExample
{
    /// <summary>
    /// 对象结构类，能枚举它的元素，可以提供一个高层的接口以允许访问者访问它的元素
    /// </summary>
    class ObjectStructure
    {
        private IList<Person> persons = new List<Person>();

        //增加
        public void Attach(Person element)
        {
            persons.Add(element);
        }

        //移除
        public void Detach(Person element)
        {
            persons.Remove(element);
        }

        //查看显示
        public void Display(Actions.Action visitor)
        {
            foreach(Person e in persons)
            {
                e.Accept(visitor);
            }
        }
    }
}
