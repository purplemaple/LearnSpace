using _3._0_DelegateEventForObserverPattern.Observers;
using _3._0_DelegateEventForObserverPattern.Subjects;

namespace _3._0_DelegateEventForObserverPattern
{
    internal class Program
    {
        /*
         * 观察者模式：即发布订阅模式(Publish/Subscribe)，定义了一种一对多的依赖关系，让多个观察者对象同时监听某一个主题对象。当这个主题对象状态发生改变时通知所有观察者，使他们能据此更新自己
         * 
         * 意义：一个模型具有两方面，且两方面互相依赖，双向耦合时，观察者模式可以解耦，让耦和的双方都依赖于抽象而非具体，从而使得各自的变化都不会影响另一方的变化
         * 
         * 适用场景：当一个对象的改变需要同时改变其他对象，且它不知道具体要同时改变多少对象时，考虑使用观察者模式
         */

        /*
         * 委托 + 事件实现观察者模式
         * 去掉 抽象观察类(Observe)，将具体观察者修改成不同类型
         */
        static void Main(string[] args)
        {
            //老板胡汉三
            Boss huhansan = new Boss();

            //看股票的同事
            StockObserver stockObserver = new StockObserver("张三", huhansan);
            //看NBA直播的同事
            NBAObserver nbaObserver = new NBAObserver("李四", huhansan);

            //将不同观察者的应对方法挂载到通知者的 Update 事件上，当通知者 Update 时，自动执行所有已挂载观察者的方法
            huhansan.Update += new UpdateEventHandler(stockObserver.CloseStockMarket);
            huhansan.Update += new UpdateEventHandler(nbaObserver.CloseNBADirectSeeding);

            //老板回来
            huhansan.SubjectState = "我胡汉三又回来了！";
            //发出通知
            huhansan.Notify();
        }
    }

    //自定义委托，无参无返回，用于声明具体通知类的 Update() 事件
    delegate void UpdateEventHandler();
}