using _2._0_ObserverPatternExample.Observers;
using _2._0_ObserverPatternExample.Subjects;

namespace _2._0_ObserverPatternExample
{
    internal class Program
    {
        /*
         * 本案例的不足之处：
         *      观察者之间可能有极大差别，各自的应对方法也天差地别，不能强制要求每一个观察者都去实现 Observe 接口(或抽象类)。
         *      如各种控件，需要监听某个相同页面，但控件制造商已经把控件封装好了，外部无法修改，自然无法随意继承或实现
         *      
         * 解决办法：使用委托 + 事件实现观察者模式，见项目 3.0
         */
        static void Main(string[] args)
        {
            //老板
            Boss huhansan = new Boss();

            //看股票的同事
            StockObserver observer1 = new StockObserver("张三", huhansan);
            StockObserver observer2 = new StockObserver("李四", huhansan);

            huhansan.Attach(observer1);
            huhansan.Attach(observer2);

            //李四没有被通知到，这里用剔除通知列表模拟
            huhansan.Detach(observer2);

            //老板回来
            huhansan.SubjectState = "我胡汉三又回来了！";
            //发出通知
            huhansan.Notify();
        }
    }
}