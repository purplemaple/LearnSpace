namespace _0._0_ConstructorIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * 模拟孙小白组装电脑
             */

            /*
             * 缺陷: 
             * 1. 所有内容都由客户自己搞定, 浪费时间和精力
             * 2. 创建对象的过程和客户端强耦合
             */
            Computer computer = new Computer();
            computer.AddPart("i5CPU");
            computer.AddPart("512GB硬盘");
            computer.AddPart("16GB内存");
            computer.AddPart("17寸显示器");
            computer.AddPart("Windows10操作系统");
            computer.ShowComputer();
        }

        public class Computer
        {
            //表示电脑的零部件集合
            private List<string> listPart = new List<string>();

            public void AddPart(string part)
            {
                listPart.Add(part);
            }

            public void ShowComputer()
            {
                foreach(var item in listPart)
                {
                    Console.WriteLine("正在安装" + item);
                }
            }
        }


    }
}