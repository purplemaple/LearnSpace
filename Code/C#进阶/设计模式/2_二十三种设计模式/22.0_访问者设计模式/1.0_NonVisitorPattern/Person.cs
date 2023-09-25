using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_NonVisitorPattern
{
    /// <summary>
    /// 抽象的 “人” 类
    /// </summary>
    abstract class Person
    {
        protected string action;
        public string Action
        {
            get => action;
            set => action = value;
        }

        //得到结论或反应
        public abstract void GetConclusion();
    }
}
