using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_StatePatternIntro
{
    /// <summary>
    /// 具体状态类，实现一个与 Context 的一个状态相关的行为
    /// </summary>
    class ConcreteStateA : State
    {
        public override void Handle(Context context)
        {
            //这里是设置 A 的下一个状态为 B
            context.State = new ConcreteStateB();
        }
    }
}
