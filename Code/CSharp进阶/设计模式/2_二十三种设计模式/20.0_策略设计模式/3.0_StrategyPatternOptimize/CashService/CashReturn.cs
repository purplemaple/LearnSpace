using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StrategyPatternOptimize.CashService
{
    class CashReturn : CashSuper
    {
        private double moneyCondition = 0;
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
