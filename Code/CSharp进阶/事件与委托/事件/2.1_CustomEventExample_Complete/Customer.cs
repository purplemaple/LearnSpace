using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._1_CustomEventExample_Complete
{
    //顾客，事件拥有者类
    public class Customer
    {
        //委托类型字段
        private OrderEventHandler orderEventHandler;

        /* 
         * 自定义的事件
         * 使用 OrderEventHandler 这个委托类型来约束，所以之后挂载事件处理器时，处理器也必须符合 OrderEventHandler 委托约束
         * Order：事件名
         * 
         * 非重：
         * 1. 由此可以看到，给事件挂载事件处理器时，实际上是挂载到了事件拥有者内部的委托字段上
         * 2. 当触发事件时，实际上是调用这个委托字段的Invoke方法，执行所有挂载的事件处理器。
         */
        public event OrderEventHandler Order
        {
            //事件处理器的添加器，类似于属性的索引器
            add
            {
                //给委托字段挂载事件处理器
                this.orderEventHandler += value;
            }

            //事件处理器的移除器，类似于属性的索引器
            remove
            {
                //给委托字段移除事件处理器
                this.orderEventHandler -= value;
            }
        }

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
            if (this.orderEventHandler != null)
            {
                OrderEventArgs e = new OrderEventArgs() { DishName = "Kongpao Chicken", Size = "large" };
                //传入参数，并且执行委托字段
                this.orderEventHandler.Invoke(this, e);
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
