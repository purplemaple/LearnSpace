using _16._0_MediatorIntro.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16._0_MediatorIntro.Colleagues
{
    abstract class Colleague
    {
        protected Mediator mediator;

        //得到中介者对象
        public Colleague(Mediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
