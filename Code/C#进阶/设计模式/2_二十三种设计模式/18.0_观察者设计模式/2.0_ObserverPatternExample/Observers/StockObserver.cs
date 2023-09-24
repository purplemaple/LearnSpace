using _2._0_ObserverPatternExample.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ObserverPatternExample.Observers
{
    class StockObserver : Observer
    {
        public StockObserver(string name, Subject sub) : base(name, sub) { }

        public override void Update()
        {
            Console.WriteLine(sub.SubjectState + " " + name + "关闭股票行情，继续工作！");
        }
    }
}
