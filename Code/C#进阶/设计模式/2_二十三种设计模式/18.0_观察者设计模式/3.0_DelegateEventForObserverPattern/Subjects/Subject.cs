using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_DelegateEventForObserverPattern.Subjects
{
    /// <summary>
    /// 通知者接口
    /// </summary>
    interface Subject
    {
        void Notify();

        string SubjectState
        {
            get;
            set;
        }
    }
}
