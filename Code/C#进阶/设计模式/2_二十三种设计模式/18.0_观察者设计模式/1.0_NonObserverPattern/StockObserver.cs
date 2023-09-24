using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_NonObserverPattern
{
    internal class StockObserver
    {
        private string name;
        //注意这里，被通知者也需要知道通知者的信息，属于双向耦合
        private Secretary sub;

        public StockObserver(string name, Secretary sub)
        {
            this.name = name;
            this.sub = sub;
        }

        public void Update()
        {
            Console.WriteLine(sub.SecretaryAction + " " +  name + "关闭股票，继续工作！");
        }
    }
}
