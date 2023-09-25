using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StrategyPatternOptimize.CashService
{
    /// <summary>
    /// 现金收费抽象类
    /// </summary>
    abstract class CashSuper
    {
        //收取现金的抽象方法，参数为原价，返回为当前价
        public abstract double acceptCash(double money);
    }
}
