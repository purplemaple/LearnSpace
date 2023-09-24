using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_DelegateEventForObserverPattern.Subjects
{
    /// <summary>
    /// 老板类
    /// </summary>
    class Boss : Subject
    {
        //声明一个 Update 事件（这里采用的是事件的简化声明方法，详见事件.md）
        public event UpdateEventHandler Update;

        private string action;


        public void Notify()
        {
            //通知时，调用Update委托事件
            Update();
        }

        public string SubjectState
        {
            get { return action; }
            set { action = value; }
        }
    }
}
