using _2._0_ObserverPatternExample.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ObserverPatternExample.Observers
{
    /// <summary>
    /// 抽象观察者
    /// </summary>
    abstract class Observer
    {
        protected string name;
        protected Subject sub;

        //同样，需要知晓通知者的信息
        public Observer(string name, Subject sub)
        {
            this.name = name;
            this.sub = sub;
        }

        public abstract void Update();
    }
}
