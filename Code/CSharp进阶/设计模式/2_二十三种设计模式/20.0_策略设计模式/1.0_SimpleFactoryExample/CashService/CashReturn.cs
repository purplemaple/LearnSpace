using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_SimpleFactoryExample.CashService
{
    /// <summary>
    /// 满减子类
    /// </summary>
    class CashReturn : CashSuper
    {
        //满减的起始标准
        private double moneyCondition = 0;
        //满足标准后的减除金额
        private double moneyReturn = 0;

        public CashReturn(string moneyCondition, string moneyReturn)
        {
            this.moneyCondition = double.Parse(moneyCondition);
            this.moneyReturn = double.Parse(moneyReturn);
        }

        public override double acceptCash(double money)
        {
            double result = money;
             if(money >= moneyCondition)
            {
                result = money - Math.Floor(money / moneyCondition) * moneyReturn;
            }

            return result;
        }
    }
}
