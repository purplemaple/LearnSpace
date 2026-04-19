using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_SimpleFactoryExample.CashService
{
    /// <summary>
    /// 无折扣，原价类
    /// </summary>
    class CashNormal : CashSuper
    {
        public override double acceptCash(double money)
        {
            return money;
        }
    }
}
