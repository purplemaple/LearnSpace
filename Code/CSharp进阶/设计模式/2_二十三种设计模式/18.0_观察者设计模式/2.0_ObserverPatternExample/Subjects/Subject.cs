using _2._0_ObserverPatternExample.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ObserverPatternExample.Subjects
{
    /// <summary>
    /// 通知者接口
    /// </summary>
    interface Subject
    {
        void Attach(Observer observer);
        void Detach(Observer observer);
        void Notify();
        string SubjectState { get; set; }
    }
}
