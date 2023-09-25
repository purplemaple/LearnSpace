using _3._0_VisitorPatternExample.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_VisitorPatternExample.Actions
{
    //失败
    class Failing : Action
    {
        public override void GetManConclusion(Man concreteElementA)
        {
            Console.WriteLine("{0}{1}时，闷头喝酒，谁也不用劝。", concreteElementA.GetType().Name, this.GetType().Name);
        }

        public override void GetWomanConclusion(Woman concreteElementB)
        {
            Console.WriteLine("{0}{1}时，眼泪汪汪，谁劝也没用。", concreteElementB.GetType().Name, this.GetType().Name);
        }
    }
}
