namespace _2._4_EventHandler
{
    /*
     * 使用自带的 EventHandler(object sender, EventArgs e) 委托来省去自定义委托的步骤
     * 注意：这里是节省了自定义委托的步骤，不是和案例 2.2 那样节省了委托字段！(这是两个不同的东西) 
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

    //使用自带的 EventHandler(object sender, EventArgs e) 委托来省去自定义委托的步骤
    //public delegate void OrderEventHandler(Customer customer, OrderEventArgs e);
}