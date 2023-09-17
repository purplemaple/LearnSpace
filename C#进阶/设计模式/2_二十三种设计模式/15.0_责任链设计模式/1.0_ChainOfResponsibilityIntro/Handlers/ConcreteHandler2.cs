﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_ChainOfResponsibilityIntro.Handlers
{
    class ConcreteHandler2 : Handler
    {
        private string Name = "总监";
        public override void HandleRequest(int request)
        {
            //如果请求码在 [10, 20) 之间则本类有权处理，否则移交给下一位
            if (request >= 10 && request < 20)
            {
                Console.WriteLine(this.Name + " 处理请求 ---> " + request);
            }
            else if (successor != null)
            {
                /* 
                 * 移交给下一位
                 * 注：successor 字段在抽象父类中
                 * 在客户端中，会设置 h1 的继承者(即successor)为 h2，h2 的继承者为 h3...以此类推，因此 h2 这里调用会传给 h3
                 */
                successor.HandleRequest(request);
            }
        }
    }
}
