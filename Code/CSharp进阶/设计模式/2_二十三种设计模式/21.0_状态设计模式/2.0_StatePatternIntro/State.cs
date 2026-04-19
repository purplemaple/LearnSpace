using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_StatePatternIntro
{
    /// <summary>
    /// State类，抽象状态类，定义一个接口以封装与Context的一个特定状态相关的行为
    /// </summary>
    abstract class State
    {
        public abstract void Handle(Context context);
    }
}
