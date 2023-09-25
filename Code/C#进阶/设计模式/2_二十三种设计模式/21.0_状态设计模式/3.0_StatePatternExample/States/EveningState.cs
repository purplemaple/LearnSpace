using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample.States
{
    /// <summary>
    /// 晚间工作状态
    /// </summary>
    public class EveningState : State
    {
        public override void WriteProgram(Work w)
        {
            if(w.TaskFinished)
            {
                //此时下班时间到了，判断任务是否完成，如果完成则转入下班状态
                w.SetState(new RestState());
                w.WriteProgram();
            }
            else
            {
                if(w.Hour < 21)
                {
                    Console.WriteLine("当前时间：{0}点 加班哦，疲惫至极", w.Hour);
                }
                else
                {
                    //超过21点，则转入睡眠工作状态
                    w.SetState(new SleepingState());
                    w.WriteProgram();
                }
            }
        }
    }
}
