using _16._0_MediatorIntro.Colleagues;
using _16._0_MediatorIntro.Mediators;

namespace _16._0_MediatorIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConcreteMediator m = new();

            //让两个具体的同事类认识中介者对象
            ConcreteColleague1 c1 = new(m);
            ConcreteColleague2 c2 = new(m);

            //让中介者认识各个具体的同时类对象
            m.Colleague1 = c1;
            m.Colleague2 = c2;

            //具体同事类对象发送的消息都是由中介者转发
            c1.Send("吃过饭科玛？");
            c2.Send("没有呢，你打算请客？");

        }
    }
}