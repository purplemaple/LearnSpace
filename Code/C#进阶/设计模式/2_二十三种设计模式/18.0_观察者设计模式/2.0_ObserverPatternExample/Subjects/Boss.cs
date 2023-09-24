using _2._0_ObserverPatternExample.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ObserverPatternExample.Subjects
{
    /// <summary>
    /// 具体的通知者不只有前台，还有老板，他们可以具有不同功能，但都实现 Subject 接口具有通知功能
    /// </summary>
    class Boss : Subject
    {
        //同事列表
        private IList<Observer> observers = new List<Observer>();
        private string action;


        //增加
        public void Attach(Observer observer)
        {
            observers.Add(observer);
        }

        //减少
        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        //通知
        public void Notify()
        {
            foreach (Observer o in observers)
            {
                o.Update();
            }
        }

        //老板状态
        public string SubjectState
        {
            get { return action; }
            set { action = value; }
        }
    }
}
