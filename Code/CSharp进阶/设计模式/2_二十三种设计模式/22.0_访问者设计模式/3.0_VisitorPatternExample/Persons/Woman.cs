using _3._0_VisitorPatternExample.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_VisitorPatternExample.Persons
{
    /// <summary>
    /// 女人
    /// </summary>
    class Woman : Person
    {
        public override void Accept(Actions.Action visitor)
        {
            /*
             * 注意这里用到了双分派技术，即在客户端中将 “具体状态” 作为参数传递给 “女人” 完成一次分派，然后 “女人” 类又将自己(this)作为参数传递到 “具体状态类” 中，完成二次分派
             */
            visitor.GetWomanConclusion(this);
        }
    }
}
