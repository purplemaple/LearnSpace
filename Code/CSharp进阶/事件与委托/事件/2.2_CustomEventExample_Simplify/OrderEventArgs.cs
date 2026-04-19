using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._2_CustomEventExample_Simplify
{
    /* 
     * 自定义的用于传递事件消息的类
     * 1. 命名应该使用事件名 + EventArgs 后缀
     * 2. 应该继承 EventArgs 基类
     * 
     * 这里包含菜品名称和菜品大小两种信息
     */
    public class OrderEventArgs : EventArgs
    {
        public string DishName { get; set; }
        public string Size { get; set; }
    }
}
