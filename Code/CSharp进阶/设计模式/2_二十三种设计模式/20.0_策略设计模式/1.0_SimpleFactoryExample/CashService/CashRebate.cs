using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_SimpleFactoryExample.CashService
{
    /// <summary>
    /// 打折收费子类
    /// </summary>
    class CashRebate : CashSuper
    {
        private double moneyRebate = 1;

        //moneyRebate：折扣率，如八折就是0.8
        public CashRebate(string moneyRebate)
        {
            this.moneyRebate = double.Parse(moneyRebate);
        }

        public override double acceptCash(double money)
        {
            return money * moneyRebate;
        }
    }
}
