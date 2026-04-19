using _3._0_VisitorPatternExample.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_VisitorPatternExample.Persons
{
    /// <summary>
    /// 抽象的 人 类
    /// </summary>
    abstract class Person
    {
        //获取状态对象
        public abstract void Accept(Actions.Action visitor);
    }
}
