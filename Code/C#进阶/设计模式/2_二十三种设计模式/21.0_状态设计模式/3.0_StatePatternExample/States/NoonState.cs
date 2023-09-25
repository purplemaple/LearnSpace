using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StatePatternExample.States
{
    /// <summary>
    /// 中午工作状态
    /// </summary>
    public class NoonState : State
    {
        public override void WriteProgram(Work w)
        {
            if(w.Hour < 13)
            {
                Console.WriteLine("当前时间：{0}点 饿了，午饭；犯困，午休。", w.Hour);
            }
            else
            {
                //超过13点，则转入下午工作状态
                w.SetState(new AfternoonState());
                w.WriteProgram();
            }
        }
    }
}
