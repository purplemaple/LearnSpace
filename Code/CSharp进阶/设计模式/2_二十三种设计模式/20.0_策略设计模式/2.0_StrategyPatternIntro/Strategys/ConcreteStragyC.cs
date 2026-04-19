using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_StrategyPatternIntro.Strategys
{
    class ConcreteStragyC : Strategy
    {
        //算法C实现
        public override void AlgorithmInterface()
        {
            Console.WriteLine("算法C实现");
        }
    }
}
