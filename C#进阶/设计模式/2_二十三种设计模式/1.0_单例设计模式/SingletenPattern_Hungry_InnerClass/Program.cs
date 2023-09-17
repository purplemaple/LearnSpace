namespace SingletenPattern_Hungry_InnerClass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }


        public class HungryClass
        {
            public static HungryClass GetSingleHungry()
            {
                return InnerClass.hungryClass;
            }

            //在饿汉类的内部写一个静态类
            //优点: 不会跟着 HungryClass 一起加载到内存中, 只有在调用时才会加载
            //缺陷: 依然能用反射攻破
            private static class InnerClass     //内部类用 private 修饰, 符合迪米特原则
            {
                //内部类中的方法用 internal 修饰, 符合迪米特原则
                internal static readonly HungryClass hungryClass = new HungryClass();
            }
        }

        

    }
}