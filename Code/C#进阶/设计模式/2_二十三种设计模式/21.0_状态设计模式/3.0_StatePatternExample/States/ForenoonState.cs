using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample.States
{
    /// <summary>
    /// 上午工作状态
    /// </summary>
    public class ForenoonState : State
    {
        public override void WriteProgram(Work w)
        {
            if(w.Hour < 12)
            {
                Console.WriteLine("当前时间：{0}点 上午工作，精神百倍", w.Hour);
            }
            else
            {
                //超过12点，则转入中午工作状态
                w.SetState(new NoonState());
                w.WriteProgram();
            }
        }
    }
}
