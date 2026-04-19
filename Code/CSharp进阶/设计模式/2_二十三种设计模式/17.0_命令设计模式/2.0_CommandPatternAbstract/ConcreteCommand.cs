using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_CommandPatternAbstract
{
    /// <summary>
    /// ConcreteCommand类，将一个接收者对象绑定与一个动作，调用接收者相应的操作，以实现 Execute
    /// </summary>
    public class ConcreteCommand : Command
    {
        public ConcreteCommand(Receiver receiver) : base(receiver) { }

        public override void Execute()
        {
            receiver.Action();
        }
    }
}
