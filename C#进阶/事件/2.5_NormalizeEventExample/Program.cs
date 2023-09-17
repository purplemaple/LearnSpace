namespace _2._5_NormalizeEventExample
{
    internal class Program
    {
        /*
         * 规范化事件案例：
         * 将之前的项目修改一下，使之符合规范
         * 1. Customer 类中 Think() 方法同时做了两件事，不符合单一职责原则
         * 2. 触发事件的方法一般命名为 On + 事件名(访问级别设置为 protected，不然又会被滥用)
         */
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
}