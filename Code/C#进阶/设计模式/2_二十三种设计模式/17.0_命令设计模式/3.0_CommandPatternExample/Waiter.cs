using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_CommandPatternExample
{
    /// <summary>
    /// 服务员类，不管用户想要什么烤肉，反正都是"命令"，只管记录订单，然后通知"烤肉师傅"即可
    /// </summary>
    public class Waiter
    {
        private Command command;

        //设置订单
        public void SetOrder(Command command)
        {
            this.command = command;
        }

        //通知执行
        public void Notify()
        {
            command.ExecuteCommand();
        }
    }
}
