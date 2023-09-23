using _2._0_MediatorExample.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MediatorExample.Countries
{
    //国家抽象类
    abstract class Country
    {
        protected UniteNations mediator;

        public Country(UniteNations mediator)
        {
            this.mediator = mediator;
        }
    }
}
