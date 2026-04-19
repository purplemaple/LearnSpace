using _2._0_MediatorExample.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_MediatorExample.Mediators
{
    //联合国机构
    abstract class UniteNations
    {
        //抽象的业务方法
        public abstract void Declare(string message, Country country);
    }
}
