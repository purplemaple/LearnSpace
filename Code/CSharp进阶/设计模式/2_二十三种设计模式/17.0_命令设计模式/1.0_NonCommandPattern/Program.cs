namespace _1._0_NonCommandPattern
{
    internal class Program
    {
        /*
         * 项目 1.0：模拟不使用命令设计模式的情况，客户直接对接烤串师傅，要什么就做什么，要多少就做多少，不记录是谁要的，也无法排队
         * 
         */
        static void Main(string[] args)
        {
            /*
             * 客户端程序与"烤串者"紧耦合，尽管简单，但僵化，后患无穷
             */
            Barbecuer boy = new Barbecuer();
            boy.BakeMutton();
            boy.BakeMutton();
            boy.BakeMutton();
            boy.BakeMutton();
            boy.BakeChickenWing();
            boy.BakeMutton();
            boy.BakeMutton();
            boy.BakeChickenWing();
        }
    }
}