using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_CommandPatternExample
{
    /// <summary>
    /// 烤羊肉串命令的实现类
    /// </summary>
    public class BakeMuttonCommand : Command
    {
        public BakeMuttonCommand(Barbecuer receiver) : base(receiver) { }

        public override void ExecuteCommand()
        {
            receiver.BakeMutton();
        }
    }
}
