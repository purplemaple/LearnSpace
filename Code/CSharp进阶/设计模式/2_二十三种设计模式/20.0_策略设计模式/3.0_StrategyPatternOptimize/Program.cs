using _3._0_StrategyPatternOptimize.CashService;
using System.Threading.Tasks;

namespace _3._0_StrategyPatternOptimize
{
    internal class Program
    {
        /*
         * 策略模式：定义了算法家族，分别封装起来，让他们之间可以互相替换，此模式可以让算法的变化不会影响到使用算法的客户
         * 
         * 意义：当不同的行为堆砌在一个类中时，就很难避免使用条件语句来选择合适的行为。将这些行为封装在一个个独立的 Strategy 类中，可以在使用这些行为的类中消除条件语句
         * 
         * 优点：简化了单元测试，因为每个算法都有自己的类，可以通过自己的接口单独测试
         */

        /*
         * 本项目采用了策略模式 + 简单工厂模式结合优化案例
         * 
         * 与单独使用简单工厂模式的比较：
         *      1. 使用 CashContext 替代了 CashFactory
         *      2. 客户端中只认识 CashContext 这一个类，而非简单工厂模式的 CashSuper 和 CashFactory 两个类都需要，耦合度更低
         *      
         * 注：这里使用两者结合的方式实现，因此看似和单独用简单工厂比较像，实际上原始的策略模式详见项目2.0。(各种策略直接在 Context 的构造中传入，没有动态的创建过程)
         */
        static void Main(string[] args)
        {
            double price = 120;
            double number = 5;
            double total = 0;
            string type = "满300反100";
            //string type = "打8折";

            CashContext csuper = new CashContext(type);

            double totalPrices = csuper.GetResult(price * number);
            total += totalPrices;

            Console.WriteLine("单价：" + price + " 数量：" + number + " 活动类型：" + type);
            Console.WriteLine("合计：" + total + "元");
        }
    }
}