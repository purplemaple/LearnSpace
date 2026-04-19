using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._4_EventHandler
{
    //服务员，事件响应器类
    public class Waiter
    {

        /*
         * 使用自带的 EventHandler 委托后后，事件响应器的参数要修改成一致
         */
        public void Action(object sender, EventArgs e)
        {
            /*
             * 由于修改了参数列表的参数类型，因此使用前得强转
             */
            Customer customer = sender as Customer;
            OrderEventArgs orderInfo = e as OrderEventArgs;

            Console.WriteLine("I Will serve you the dish -{0}.", orderInfo.DishName);
            double price = 10;
            switch (orderInfo.Size)
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
