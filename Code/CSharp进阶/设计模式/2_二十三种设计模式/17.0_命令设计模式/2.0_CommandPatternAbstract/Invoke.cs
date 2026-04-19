using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_CommandPatternAbstract
{
    /// <summary>
    /// Invoke类，要求该命令执行这个请求
    /// </summary>
    public class Invoke
    {
        private Command command;

        public void SetCommand(Command command)
        {
            this.command = command; 
        }

        public void ExecuteCommand()
        {
            command.Execute();
        }
    }
}
