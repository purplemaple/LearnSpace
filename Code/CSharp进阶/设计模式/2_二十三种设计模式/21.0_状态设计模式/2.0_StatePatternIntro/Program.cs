namespace _2._0_StatePatternIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //设置 Context 的初始状态为 ConcreteStateA
            Context c = new Context(new ConcreteStateA());

            c.Request();
            c.Request();
            c.Request();
            c.Request();
            c.Request();
        }
    }
}