using _1._0_ChainOfResponsibilityIntro.Handlers;

namespace _1._0_ChainOfResponsibilityIntro
{
    /*
     * 责任链设计模式引入
     * 本案例模拟一个向经理、总监、总经理逐级申请过程
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            Handler h1 = new ConcreteHandler1();
            Handler h2 = new ConcreteHandler2();
            Handler h3 = new ConcreteHandler3();

            //设置责任链的上下家
            h1.SetSuccessor(h2);
            h2.SetSuccessor(h3);

            int[] requests = { 2, 5, 14, 22, 18, 3, 27, 20 };

            foreach(int item in  requests)
            {
                h1.HandleRequest(item);
            }
        }
    }
}