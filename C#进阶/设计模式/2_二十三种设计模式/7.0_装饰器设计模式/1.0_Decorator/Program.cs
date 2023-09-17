namespace _1._0_Decorator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //需求: 一杯奶茶 + 两份布丁 + 一份珍珠
            MilkTea milkTea = new MilkTea();
            //创建两份布丁对象
            Buding buding1 = new Buding();
            Buding buding2 = new Buding();
            //创建珍珠对象
            ZhenZhu zhenZhu = new ZhenZhu();

            //给奶茶添加布丁1
            buding1.SetComponent(milkTea);

            //给奶茶整体添加布丁2  -->  此时 buding1 已成为一个整体, 包含布丁1和奶茶, 所以传参时传入的是 buding1
            buding2.SetComponent(buding1);

            //给奶茶整体添加珍珠  -->  同上, 此时 buding2 是奶茶整体
            zhenZhu.SetComponent(buding2);

            Console.WriteLine("最终价格为: " + zhenZhu.Cost());
        }

        public abstract class YinLiao
        {
            public abstract double Cost();
        }

        public class MilkTea : YinLiao
        {
            public override double Cost()
            {
                Console.WriteLine("奶茶10块钱一杯");
                return 10;
            }
        }

        public class FruitTea : YinLiao
        {
            public override double Cost()
            {
                Console.WriteLine("水果茶20块钱一杯");
                return 20;
            }
        }

        public class SoDaTea : YinLiao
        {
            public override double Cost()
            {
                Console.WriteLine("苏打饮料30块钱一杯");
                return 30;
            }
        }

        /*
         * 核心1: Decorator 类
         * 用于给 YinLiao 类添加 Component (组件)  -->  相当于套个壳, 原本只有奶茶, 添加布丁后, 奶茶和布丁成为新的整体, 赋值回 yinLiao 对象
         */
        public abstract class Decorator : YinLiao
        {
            //添加父类引用
            private YinLiao yinLiao;

            //通过Set方法, 给他赋值
            public void SetComponent(YinLiao yinLiao)
            {
                this.yinLiao = yinLiao;
            }

            public override double Cost()
            {
                return this.yinLiao.Cost();
            }
        }

        public class Buding : Decorator
        {
            private static double money = 5;
            public override double Cost()
            {
                Console.WriteLine("布丁5块");
                /*
                 * 核心2: base.Cost() + money
                 * 计算价格时, 需要先执行父类的方法拿到之前整体的价格(已经添加若干或没有添加配料), 然后加上自己的价格  -->  类递归
                 */
                return base.Cost() + money;
            }
        }

        public class XianCao : Decorator
        {
            private static double money = 6;
            public override double Cost()
            {
                Console.WriteLine("仙草6块钱");
                return base.Cost() + money;
            }
        }

        public class ZhenZhu : Decorator
        {
            private static double money = 7;
            public override double Cost()
            {
                Console.WriteLine("珍珠7块钱");
                return base.Cost() + money;
            }
        }

    }  
}