namespace _3._0_CommandPatternExample
{
    internal class Program
    {
        /*
         * 项目3.0：使用了命令模式，但还是紧耦合
         */
        static void Main(string[] args)
        {
            //预先准备
            Barbecuer boy = new Barbecuer();
            Command bakeMuttonCommand1 = new BakeMuttonCommand(boy);
            Command bakeMuttonCommand2 = new BakeMuttonCommand(boy);
            Command bakeChickWingCommand1 = new BakeChickenWingCommand(boy);
            Waiter waiter = new Waiter();

            //开始营业
            waiter.SetOrder(bakeMuttonCommand1);
            waiter.Notify();

            waiter.SetOrder(bakeMuttonCommand2);
            waiter.Notify();

            waiter.SetOrder(bakeChickWingCommand1);
            waiter.Notify();

            /*
             * 这种紧耦合的缺点：
             *      1. 每点一个菜就通知一次不合实际，应该是全部点完后一次通知
             *      2. 没有判断菜品是否有剩余的功能   -->  服务员
             *      3. 没有记录日志   -->  服务员
             *      4. 没有点完菜后修改或取消的功能   -->  服务员
             */
        }
    }
}