using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._4_EventHandler
{
    //顾客，事件拥有者类
    public class Customer
    {

        //public event OrderEventHandler Order;

        //使用自带的 EventHandler(object sender, EventArgs e) 委托
        public event EventHandler Order;

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
