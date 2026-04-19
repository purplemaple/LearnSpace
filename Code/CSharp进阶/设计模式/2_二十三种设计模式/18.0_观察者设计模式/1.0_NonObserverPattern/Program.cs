namespace _1._0_NonObserverPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //前台小姐
            Secretary secretary = new Secretary();
            //看股票的同事
            StockObserver observer1 = new StockObserver("张三", secretary);
            StockObserver observer2 = new StockObserver("李四", secretary);

            //前台记下需要通知的同事
            secretary.Attach(observer1);
            secretary.Attach(observer2);

            //发现老板回来
            secretary.SecretaryAction = "老板回来了！";

            //通知所有同事
            secretary.Notify();
        }
    }
}