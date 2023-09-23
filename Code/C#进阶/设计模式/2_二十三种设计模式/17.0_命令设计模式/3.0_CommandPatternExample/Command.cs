using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_CommandPatternExample
{
    /// <summary>
    /// 抽象命令类，只需要确定“烤肉师傅”是哪一位
    /// </summary>
    public abstract class Command
    {
        protected Barbecuer receiver;

        public Command(Barbecuer receiver)
        {
            this.receiver = receiver;
        }

        //执行命令
        abstract public void ExecuteCommand();
    }
}
