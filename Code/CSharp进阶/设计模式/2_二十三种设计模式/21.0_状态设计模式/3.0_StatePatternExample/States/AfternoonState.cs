using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample.States
{
    /// <summary>
    /// 下午工作状态
    /// </summary>
    public class AfternoonState : State
    {
        public override void WriteProgram(Work w)
        {
            if(w.Hour < 17)
            {
                Console.WriteLine("当前时间：{0}点 下午状态还不错，继续努力", w.Hour);
            }
            else
            {
                //超过17点，则转入傍晚工作状态
                w.SetState(new EveningState());
                w.WriteProgram();
            }
        }
    }
}
