using _16._0_MediatorIntro.Colleagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16._0_MediatorIntro.Mediators
{
    abstract class Mediator
    {
        //抽象的发送消息方法，得到同事对象和发送消息
        public abstract void Send(string message, Colleague colleague);
    }
}
