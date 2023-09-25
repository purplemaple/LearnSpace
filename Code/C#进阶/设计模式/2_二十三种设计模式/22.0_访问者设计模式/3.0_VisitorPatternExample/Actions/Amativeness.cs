using _3._0_VisitorPatternExample.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_VisitorPatternExample.Actions
{
    //恋爱
    class Amativeness : Action
    {
        public override void GetManConclusion(Man concreteElementA)
        {
            Console.WriteLine("{0}{1}时，凡事不懂也要装懂。", concreteElementA.GetType().Name, this.GetType().Name);
        }

        public override void GetWomanConclusion(Woman concreteElementB)
        {
            Console.WriteLine("{0}{1}时，遇事懂也要装作不懂。", concreteElementB.GetType().Name, this.GetType().Name);
        }
    }
}
