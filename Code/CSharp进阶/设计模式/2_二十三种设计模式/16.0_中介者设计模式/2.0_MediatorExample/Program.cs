using _2._0_MediatorExample.Countries;
using _2._0_MediatorExample.Mediators;

namespace _2._0_MediatorExample
{
    internal class Program
    {
        /*
         * 中介者设计模式：用一个中介对象来封装一系列的对象交互，中介者使各对象不需要显式地相互引用，从而使其松耦合，而且可以独立地改变它们之间的交互
         * 
         * 优点：
         *      1. 降低了各个业务对象间的耦合，使得可以独立地改变和复用各个业务对象和中介者
         *      2. 将对象之间的协作关系都封装到中介者中，可以站在一个更全面更宏观的角度去看待整个系统
         *      
         * 缺点：
         *      1. 复杂性并没有减少，只是从业务对象的交互复杂性转变成了中介者的复杂性，因此中介者会变得臃肿（中介者需要知道所有业务对象的消息）
         *      
         * 因此，当系统出现了'多对多'交互复杂的对象群时，不要着急使用中介者模式，而是反思系统设计是否合理
         * 
         * 现实中的使用案例：Form窗体、WPF窗体、Web页面等，控件就是具体业务对象，它们不需要知道还有什么其他控件，而是由窗体对象充当中介者统一管理
         */
        /*
         * 本案例模拟联合国机构(中介者)，处理两国关系(业务对象)
         */
        static void Main(string[] args)
        {
            UnitedNationsSecurityCouncil UNSC = new();

            USA c1 = new(UNSC);
            Iraq c2 = new(UNSC);

            UNSC.Usa = c1;
            UNSC.Iraq = c2;

            c1.Declare("不要研制核武器，否则就要发动战争！");
            c2.Declare("我们没有核武器，也不怕侵略！");
        }
    }
}