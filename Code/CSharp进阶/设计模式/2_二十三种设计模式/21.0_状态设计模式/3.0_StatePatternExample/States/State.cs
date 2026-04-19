using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample.States
{
    //抽象状态
    public abstract class State
    {
        public abstract void WriteProgram(Work w);
    }
}
