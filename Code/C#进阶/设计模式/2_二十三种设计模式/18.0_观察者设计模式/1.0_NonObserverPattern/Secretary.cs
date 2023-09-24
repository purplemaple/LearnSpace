using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_NonObserverPattern
{
    /// <summary>
    /// 秘书类，模拟观察者
    /// </summary>
    internal class Secretary
    {
        //同事列表
        private IList<StockObserver> observers = new List<StockObserver>();
        private string action;

        //增加
        public void Attach(StockObserver observer)
        {
            observers.Add(observer);
        }

        //通知
        public void Notify()
        {
            foreach (StockObserver o in observers)
            {
                o.Update();
            }
        }

        //前台状态
        public string SecretaryAction
        {
            get { return action; }
            set { action = value; }
        }
    }
}
