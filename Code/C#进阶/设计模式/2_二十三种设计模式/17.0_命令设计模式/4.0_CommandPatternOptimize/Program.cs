namespace _4._0_CommandPatternOptimize
{
    internal class Program
    {
        /*
         * 命令模式：将一个请求封装为一个对象，从而使你可用不同的请求对客户进行参数化；对请求排队或记录请求日志，以及支持可撤销的操作
         * 
         *      优点：
         *          1. 能较容易地设计一个命令队列
         *          2. 能较容易地将命令记录日志
         *          3. 允许接受请求的一方决定是否接受请求
         *          4. 能较容易地实现对请求的撤销和重做
         *          5. 以后增加新命令时易扩展
         */
        static void Main(string[] args)
        {
            //预先准备
            Barbecuer boy = new Barbecuer();
            Command bakeMuttonCommand1 = new BakeMuttonCommand(boy);
            Command bakeMuttonCommand2 = new BakeMuttonCommand(boy);
            Command bakeChickWingCommand1 = new BakeChickenWingCommand(boy);
            Waiter waiter = new Waiter();

            //开门营业 点菜
            waiter.SetOrder(bakeMuttonCommand1);
            waiter.SetOrder(bakeMuttonCommand2);
            waiter.SetOrder(bakeChickWingCommand1);

            //点菜完毕，通知厨房
            waiter.Notify();
        }
    }
}