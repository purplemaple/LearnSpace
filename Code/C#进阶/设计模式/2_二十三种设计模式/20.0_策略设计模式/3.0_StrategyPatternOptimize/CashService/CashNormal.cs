using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StrategyPatternOptimize.CashService
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
