using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._0_ChainOfResponsibilityOptimize.Handlers
{
    //管理者
    abstract class Manager
    {
        protected string name;
        //管理者的上级(即责任链的下一环，拥有更高的权柄)
        protected Manager superior;

        public Manager(string name)
        {
            this.name = name;
        }

        //设置管理者的上级
        public void SetSuperior(Manager superior)
        {
            this.superior = superior;
        }

        //申请请求的抽象方法
        abstract public void RequestApplications(Request request);
    }
}
