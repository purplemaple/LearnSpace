using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._3_NonEventExample
{
    //顾客，事件拥有者类
    public class Customer
    {
        //public event OrderEventHandler Order;

        //将 event 关键字删除，这是 Order 就不是事件，而是一个委托了，其他地方不变，看似程序能正常运行
        public OrderEventHandler Order;

        //账单
        public double Bill { get; set; }
        public void PayTheBill()
        {
            Console.WriteLine("I Will pay ${0}.", this.Bill);
        }

        public void WalkIn()
        {
            Console.WriteLine("Walk into the restaurant");
        }

        public void SitDown()
        {
            Console.WriteLine("Sit down");
        }

        public void Think()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Let me think...");
                Thread.Sleep(1000);
            }

            //当委托字段为空时表示未挂载事件处理器
            /*if (this.orderEventHandler != null)
            {
                OrderEventArgs e = new OrderEventArgs() { DishName = "Kongpao Chicken", Size = "large" };
                //传入参数，并且执行委托字段
                this.orderEventHandler.Invoke(this, e);
            }*/

            /*
             * 注：当不使用简化方法声明事件时，不能将事件放在除 "+=" 和 "-=" 号之外的操作符的左边，也就是不能像下面这样 this.Order != null 以及 this.Order.Invoke()
             * 但使用简化声明法声明事件时却可以，这是微软的锅，前后语法不一致
             * 
             * 规定事件只能使用 += 和 -= 操作符是为了安全，不让 "非事件拥有者" 来随意操作 "事件拥有者" 的事件(详见案例2.3)
             * 这里去做非空比较以及调用 Invoke() 方法纯属不得已而为之，因为简化声明时，我们没有手动声明一个委托字段，因此不能拿委托字段来判空和调用 Invoke()
             */
            if (this.Order != null)
            {
                OrderEventArgs e = new OrderEventArgs() { DishName = "Kongpao Chicken", Size = "large" };
                this.Order.Invoke(this, e);
            }
        }

        public void Action()
        {
            Console.ReadLine();
            this.WalkIn();
            this.SitDown();
            this.Think();
        }
    }
}
