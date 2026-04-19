namespace _2._2_CustomEventExample_Simplify
{
    /*
     * 自定义委托案例2：事件的简化声明格式
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Waiter waiter = new Waiter();
            //顾客的点单事件挂载服务员的 Action 事件处理器
            customer.Order += waiter.Action;
            customer.Action();
            customer.PayTheBill();
        }
    }

    //如果委托是为了声明某个事件而准备的，那么命名应该使用事件名 +  EventHandler 做后缀
    public delegate void OrderEventHandler(Customer customer, OrderEventArgs e);
}