namespace _2._0_MementoPatternIntro
{
    internal class Program
    {
        /*
         * 备忘录模式的基本代码
         */
        static void Main(string[] args)
        {
            //Originator 初始状态，状态属性初始为 "On"
            Originator originator = new Originator();
            originator.State = "On";
            originator.Show();

            //保存状态，由于有了封装，可以隐藏 Originator 的实现细节
            Caretaker caretaker = new Caretaker();
            caretaker.MyMemento = originator.CreateMemento();

            //Originator 改变了状态属性为 "Off"
            originator.State = "Of";
            originator.Show();

            //恢复初始状态
            originator.SetMemento(caretaker.MyMemento);
            originator.Show();
        }
    }
}