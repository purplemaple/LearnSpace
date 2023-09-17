﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._5_NormalizeEventExample
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

            //封装事件相关语句
            this.OnOrder("Kongpao Chicken", "large");
        }

        //从之前的 Think 方法中提取出触发事件的语句，并封装成 OnOrder 方法(注意被 protected 修饰，防止滥用)(子类可以访问 protected)
        protected void OnOrder(string dishName, string size)
        {
            if (this.Order != null)
            {
                OrderEventArgs e = new OrderEventArgs() { DishName = dishName, Size = size };
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
