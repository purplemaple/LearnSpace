using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_ChainOfResponsibilityIntro.Handlers
{
    abstract class Handler
    {

        protected Handler successor;

        //设置继承者，也是一个Handler对象
        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        /// <summary>
        /// 处理请求的抽象方法
        /// </summary>
        /// <param name="request">请求码，实现类据此判断自己是否有权限处理本请求</param>
        public abstract void HandleRequest(int request);
    }
}
