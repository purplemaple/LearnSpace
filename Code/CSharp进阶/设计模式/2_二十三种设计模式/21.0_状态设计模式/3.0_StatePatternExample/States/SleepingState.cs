using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample.States
{
    /// <summary>
    /// 睡眠状态
    /// </summary>
    public class SleepingState : State
    {
        public override void WriteProgram(Work w)
        {
            Console.WriteLine("当前时间：{0}点不行了，睡着了。", w.Hour);
        }
    }
}
