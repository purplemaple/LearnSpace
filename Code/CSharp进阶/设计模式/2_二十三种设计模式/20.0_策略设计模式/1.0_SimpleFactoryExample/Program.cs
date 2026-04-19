using _1._0_SimpleFactoryExample.CashService;

namespace _1._0_SimpleFactoryExample
{
    internal class Program
    {
        /*
         * 本项目演示直接使用简单工厂模式的情况
         * 注：只使用简单工厂模式，未使用策略模式。策略模式详见项目 3.0
         */
        static void Main(string[] args)
        {
            double price = 120;
            double number = 5;
            double total = 0;
            //string type = "满300反100";
            string type = "打8折";

            CashSuper csuper = CashFactory.createCashAccept(type);
            double totalPrices = csuper.acceptCash(price * number) ;

            total += totalPrices;

            Console.WriteLine("单价：" + price + " 数量：" + number + " 活动类型：" + type);
            Console.WriteLine("合计：" + total + "元");
        }
    }
}