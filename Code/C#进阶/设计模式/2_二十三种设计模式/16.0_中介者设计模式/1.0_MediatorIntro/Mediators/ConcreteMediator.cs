using _16._0_MediatorIntro.Colleagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16._0_MediatorIntro.Mediators
{
    class ConcreteMediator : Mediator
    { 
        //需要了解所有具体的同事对象
        private ConcreteColleague1 _colleague1;
        private ConcreteColleague2 _colleague2;
        public ConcreteColleague1 Colleague1 { set => _colleague1 = value; }
        public ConcreteColleague2 Colleague2 { set => _colleague2 = value; }

        //重写发送消息的方法，根据对象做出判断，通知对象
        public override void Send(string message, Colleague colleague)
        {
            if(colleague == _colleague1)
            {
                _colleague2.Notify(message);
            }
            else
            {
                _colleague1.Notify(message);
            }
        }
    }
}
