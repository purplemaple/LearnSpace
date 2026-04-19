using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ChainOfResponsibilityOptimize.Handlers
{
    //总监
    class MajorManager : Manager
    {
        public MajorManager(string name) : base(name)
        {
            
        }

        public override void RequestApplications(Request request)
        {
            if (request.RequestType.Equals("请假") && request.Count <= 5)
            {
                Console.WriteLine(name + " : " + request.RequestContent + "，数量" + request.Count + "，被批准");
            }
            else
            {
                //当前无权审批，尝试递交给上级
                if (superior != null)
                {
                    superior.RequestApplications(request);
                }
            }
        }
    }
}
