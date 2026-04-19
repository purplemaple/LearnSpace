using _3._0_DelegateEventForObserverPattern.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_DelegateEventForObserverPattern.Observers
{
    /// <summary>
    /// 看股票的同事
    /// </summary>
    class StockObserver
    {
        private string name;
        private Subject sub;

        public StockObserver(string name, Subject sub)
        {
            this.name = name;
            this.sub = sub;
        }

        //关闭股票行情
        public void CloseStockMarket()
        {
            Console.WriteLine(sub.SubjectState + " " + name + "关闭股票行情，继续工作！");
        }
    }
}
