namespace _4._0_MulticastDelegateExample
{
    /*
     * 多播委托案例
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            Student stu1 = new Student() { ID = 1, PenColor = ConsoleColor.Yellow };
            Student stu2 = new Student() { ID = 2, PenColor = ConsoleColor.Green };
            Student stu3 = new Student() { ID = 3, PenColor = ConsoleColor.Red };

            Action action1 = new Action(stu1.DoHomework);
            Action action2 = new Action(stu2.DoHomework);
            Action action3 = new Action(stu3.DoHomework);

            /* 单播委托：一个委托只封装一个方法
            action1();
            action2();
            action3();*/

            /* 多播委托：一个委托封装多个方法
            action1 += action2;
            action1 += action3;

            action1();*/

            //隐式异步调用，使用委托自带的 BeginInvoke 方法 (注：仅用于.Net Framework平台， .Net Core 平台已不支持)
            /*action1.BeginInvoke(null, null);
            action2.BeginInvoke(null, null);
            action3.BeginInvoke(null, null);*/

            //显示异步调用，使用Task
            //1. 一般写法
            /*Task task1 = new Task(action1);
            Task task2 = new Task(action2);
            Task task3 = new Task(action3);

            task1.Start();
            task2.Start();
            task3.Start();*/

            //2. 简写法
            /*Task.Run(action1);
            Task.Run(action2);
            Task.Run(action3);*/

            for (int i = 0;i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Main thread{0}.", i);
                Thread.Sleep(1000);
            }
        }
    }
}