using _2._0_StrategyPatternIntro.Strategys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_StrategyPatternIntro
{
    /// <summary>
    /// 上下文类，内含一个策略类的的引用和一个调用接口，客户通过此类的调用接口调用策略类
    /// </summary>
    internal class Context
    {
        Strategy strategy;

        public Context(Strategy strategy)
        {
            this.strategy = strategy;
        }

        //上下文接口
        public void ContextInterface()
        {
            //调用具体策略类的算法
            strategy.AlgorithmInterface();
        }
    }
}
