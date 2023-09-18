using System.Timers;
namespace _1._1_SimpleEventExample_1
{
    /*
     * 如果想拿一个方法去订阅一个事件，那么这个方法与事件就必须遵循同一个约定，这个约定由委托类型来规范
     * 
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Timers.Timer timer = new();
            timer.Interval = 1000;

            Boy boy = new Boy();
            Girl girl = new Girl();
            /*
             * 订阅事件的操作符： "+=" ，左边写事件，右边写事件响应者的事件处理器
             * 如果想拿一个方法去订阅一个事件，那么这个方法与事件就必须遵循同一个约定，这个约定就是一个委托类型
             * 这里采用 vs 自动补全写法，可以发现委托类型的参数一个是 object? 类型，一个是 ElapsedEventArgs类型
             */
            timer.Elapsed += boy.JumpAction;        //boy：事件响应者     Action：事件处理器
            timer.Elapsed += girl.SingAction;

            timer.Start();
            Console.ReadLine();
        }
    }

    class Boy
    {
        /*
         * 注：事件的拥有者、事件的Source、事件的发送者指的都是同一个东西，只是叫法不同
         * 所以下面这里 sender(发送者) 指的就是事件的发送者，即常说的事件的拥有者
         */
        internal void JumpAction(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Jump!");
        }
    }

    class Girl
    {
        internal void SingAction(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Sing!");
        }
    }
}