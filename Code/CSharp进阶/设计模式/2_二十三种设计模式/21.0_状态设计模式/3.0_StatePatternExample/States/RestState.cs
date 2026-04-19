using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample.States
{
    /// <summary>
    /// 下班休息状态
    /// </summary>
    public class RestState : State
    {
        public override void WriteProgram(Work w)
        {
            Console.WriteLine("当前时间：{0}点下班回家了", w.Hour);
        }
    }
}
