using _3._0_DelegateEventForObserverPattern.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._0_DelegateEventForObserverPattern.Observers
{
    /// <summary>
    /// 看NBA的同事
    /// </summary>
    class NBAObserver
    {
        private string name;
        private Subject sub;

        public NBAObserver(string name, Subject sub)
        {
            this.name = name;
            this.sub = sub;
        }

        //关闭NBA直播
        public void CloseNBADirectSeeding()
        {
            Console.WriteLine(sub.SubjectState + " " + name + "关闭NBA直播，继续工作！");
        }
    }
}
