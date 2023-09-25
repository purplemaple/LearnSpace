using _3._0_VisitorPatternExample.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_VisitorPatternExample.Actions
{
    /// <summary>
    /// 抽象状态类
    /// </summary>
    abstract class Action
    {
        //得到男人结论或反应
        public abstract void GetManConclusion(Man concreteElementA);
        public abstract void GetWomanConclusion(Woman concreteElementB);
    }
}
