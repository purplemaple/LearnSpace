using _2._0_MediatorExample.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MediatorExample.Countries
{
    //美国
    class USA : Country
    {
        public USA(UniteNations mediator) : base(mediator)
        {

        }

        //声明
        public void Declare(string message)
        {
            mediator.Declare(message, this);
        }

        //获得消息
        public void GetMessage(string message)
        {
            Console.WriteLine("美国获得对方消息：" + message);
        }
    }
}
