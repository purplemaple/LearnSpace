namespace _2._3_NonEventExample
{
    /*
     * 不使用事件来包装，而是直接使用委托字段的案例：
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Waiter waiter = new Waiter();
            //顾客的点单事件挂载服务员的 Action 事件处理器
            customer.Order += waiter.Action;
            //customer.Action();
            //customer.PayTheBill();

            //演示不使用事件包装委托：将 Customer 类中定义事件时的关键字 event 删除，此时 Order 变成纯粹的委托字段，其他地方不变

            OrderEventArgs e1 = new OrderEventArgs() { DishName = "Manhanquanxi", Size = "large"};
            OrderEventArgs e2 = new OrderEventArgs() { DishName = "Beer", Size = "small"};

            /* 
             * 注意：这里又来了一个 badCustomer 顾客，他原本也能拥有自己的 Order 事件
             * 但这里由于没有对 Customer 内的委托字段进行封装，因此 badCustomer 能直接调用 customer 对象的 Order 委托！
             * 相当于用户2拿用户1的账号点了两个菜，账全算在用户1头上
             * 
             * 当用 event 修饰委托字段时，委托字段 Order 就被封装成了事件，又因为事件只能用于 += 和 -= 操作符，因此不能像这里这样使用 .Invoke() 调用，能防止委托字段被滥用
             */
            Customer badCustomer = new Customer();
            badCustomer.Order += waiter.Action;
            badCustomer.Order.Invoke(customer, e1);
            badCustomer.Order.Invoke(customer, e2);

            customer.PayTheBill();

        }
    }

    //如果委托是为了声明某个事件而准备的，那么命名应该使用事件名 +  EventHandler 做后缀
    public delegate void OrderEventHandler(Customer customer, OrderEventArgs e);
}