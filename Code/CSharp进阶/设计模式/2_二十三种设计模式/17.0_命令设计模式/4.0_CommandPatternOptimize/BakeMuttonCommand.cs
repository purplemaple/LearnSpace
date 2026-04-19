using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._0_CommandPatternOptimize
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
