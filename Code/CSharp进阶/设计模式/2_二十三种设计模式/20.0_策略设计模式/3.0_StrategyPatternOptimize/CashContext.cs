using _3._0_StrategyPatternOptimize.CashService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_StrategyPatternOptimize
{
    class CashContext
    {
        private CashSuper cs;

        //将实例化具体策略的过程从客户端转移至本类中 ---> 简单工厂的应用 (可以再用反射 + 抽象工厂优化)
        public CashContext(string type)
        {
            switch (type)
            {
                default:
                case "正常收费":
                    cs = new CashNormal();
                    break;
                case "满300反100":
                    cs = new CashReturn("300", "100");
                    break;
                case "打8折":
                    cs = new CashRebate("0.8");
                    break;
            }
        }

        public double GetResult(double money)
        {
            //根据收费策略的不同，获得计算结构
            return cs.acceptCash(money);
        }
    }
}
