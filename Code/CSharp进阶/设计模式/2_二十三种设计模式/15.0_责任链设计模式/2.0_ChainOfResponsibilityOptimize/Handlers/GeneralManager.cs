using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ChainOfResponsibilityOptimize.Handlers
{
    //总经理
    class GeneralManager : Manager
    {
        public GeneralManager(string name) : base(name)
        {
            
        }

        public override void RequestApplications(Request request)
        {
            /* 
             * 总经理有权处理任意天数的假期，以及任意金额的加薪
             * 注：虽然这里代码写的 500 以上的加薪被否决了，但这请求还是被处理了(即被驳回)，而不是再往上递交
             */
            if (request.RequestType.Equals("请假"))
            {
                Console.WriteLine(name + " : " + request.RequestContent + "，数量" + request.Count + "，被批准");
            }
            else if (request.RequestType.Equals("加薪") && request.Count <= 500)
            {
                Console.WriteLine(name + " : " + request.RequestContent + "，数量" + request.Count + "，被批准");
            }
            else if (request.RequestType.Equals("加薪") && request.Count > 500)
            {
                Console.WriteLine(name + " : " + request.RequestContent + "，数量" + request.Count + "，再说吧");
            }
        }
    }
}
