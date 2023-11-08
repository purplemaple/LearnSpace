namespace _2._1_CustomEventExample_Complete
{
    /*
     * 事件是基于委托的
     * 1. 事件需要委托类型来做约束，响应者的事件处理器必须与这个约束匹配上，才能订阅这个事件(既规定了事件能发送什么样的消息给响应者，也规定了响应者能收到什么样的消息)
     * 2. 当响应者提供了事件处理器时，总得找个地方将这个处理器方法保存或记录下来，而能够记录或引用方法的任务，只有委托能做到
     * 
     * 因此，事件无论存表层约束还是底层实现，都依赖于委托
     */
    /*
     * 自定义事件案例1：事件的完整声明格式
     */
    class Program
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