using _2._0_ObserverPatternExample.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ObserverPatternExample.Subjects
{
    class Secretary : Subject
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

        //秘书状态
        public string SubjectState
        {
            get { return action; }
            set { action = value; }
        }
    }
}
