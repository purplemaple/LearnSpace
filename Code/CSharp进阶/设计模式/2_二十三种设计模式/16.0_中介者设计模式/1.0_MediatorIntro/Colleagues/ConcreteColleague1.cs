using _16._0_MediatorIntro.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16._0_MediatorIntro.Colleagues
{
    class ConcreteColleague1 : Colleague
    {
        public ConcreteColleague1(Mediator mediator) : base(mediator)
        {
            
        }

        public void Send(string message)
        {
            //发送消息时是有中介对象发出
            mediator.Send(message, this);
        }

        public void Notify(string message)
        {
            Console.WriteLine("同事1得到消息：" + message);
        }
    }
}
