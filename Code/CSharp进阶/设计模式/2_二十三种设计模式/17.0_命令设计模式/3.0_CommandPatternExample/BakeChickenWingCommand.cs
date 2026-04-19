using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_CommandPatternExample
{
    /// <summary>
    /// 烤鸡翅的命令实现类
    /// </summary>
    public class BakeChickenWingCommand : Command
    {
        public BakeChickenWingCommand(Barbecuer receiver) : base(receiver) { }

        public override void ExecuteCommand()
        {
            receiver.BakeChickWing();
        }

    }
}
