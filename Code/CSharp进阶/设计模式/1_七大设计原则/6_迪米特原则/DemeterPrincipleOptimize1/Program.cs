namespace DemeterPrincipleOptimize1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //实现 人 关机 的案例

            //对象一: 电脑 --> 关机相关的行为
            //对象二: 人 --> 调用电脑关机相关的行为
        }

        public class Computer
        {
            //1. 保存当前程序
            private void SaveCurrentTask()
            {
                Console.WriteLine("保存当前程序");
            }

            //2. 关闭屏幕
            private void CloseScreen()
            {
                Console.WriteLine("关闭屏幕");
            }

            //3. 关闭电源
            private void ShutSown()
            {
                Console.WriteLine("关闭电源");
            }

            public void CloseComputer()
            {
                this.SaveCurrentTask();
                this.CloseScreen();
                this.ShutSown();
            }
        }

        public class Person
        {
            public void CloseComputer(Computer computer)
            {
                /*
                //不符合迪米特原则
                //存在的问题: 如果关机需要三十步, Computer提供30个相关的方法, 并且每个方法之间都有相应的绝对顺序
                computer.SaveCurrentTask();
                computer.CloseScreen();
                computer.ShutSown();
                */

                computer.CloseComputer();
            }
        }

    }
}