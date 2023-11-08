using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._3_NonEventExample
{
    //服务员，事件响应器类
    public class Waiter
    {
        /* 
         * 因为事件响应器也必须符合我们自定义的委托类型 OrderEventHandler(Customer customer, OrderEventArgs e)，因此参数列表必须是这两个
         * 
         * Customer：是自定义的顾客类，也就是事件的拥有者
         * OrderEventArgs：是自定义的事件消息类，传递一些响应事件的必须消息，因此下面才可以拿到菜品的名称与大小
         */
        public void Action(Customer customer, OrderEventArgs e)
        {
            Console.WriteLine("I Will serve you the dish -{0}.", e.DishName);
            double price = 10;
            switch (e.Size)
            {
                case "small":
                    price *= 0.5;
                    break;
                case "large":
                    price *= 1.5;
                    break;
                default:
                    break;
            }

            customer.Bill += price;
        }
    }
}
