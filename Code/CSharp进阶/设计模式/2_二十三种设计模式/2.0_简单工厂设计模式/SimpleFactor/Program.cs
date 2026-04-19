namespace SimpleFactor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //写一个简单的项目, 来实现计算器加减乘除功能

            //4个对象: 加减乘除

            Console.WriteLine("请输入操作数1: ");
            double d1 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("请输入操作数2: ");
            double d2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("请输入操作符: ");
            string oper = Console.ReadLine();

            /*
            //oper的不同, 创建不同的计算对象
            ICaculator caculator = null;
            switch (oper)
            {
                default:
                case "+":
                    caculator = new Add();
                    break;
                case "-":
                    caculator = new Sub();
                    break;
                case "*":
                    caculator = new Mul();
                    break;
                case "/":
                    caculator = new Div();
                    break;
            }
            double res = caculator.GetResult(d1, d2);
            Console.WriteLine(res);
            */

            
            //静态工厂方法(简单工厂模式): 实际上就是把创建对象的过程, 封装到静态方法中, 在客户端直接调用, 实现了客户端和创建对象的解耦
            ICaculator caculator = CalFactory.GetCalculator(oper);
            double res = caculator.GetResult(d1, d2);
            Console.WriteLine(res);

        }

        public class CalFactory
        {
            public static ICaculator GetCalculator(string oper)
            {
                ICaculator caculator = null;
                switch (oper)
                {
                    default:
                    case "+":
                        caculator = new Add();
                        break;
                    case "-":
                        caculator = new Sub();
                        break;
                    case "*":
                        caculator = new Mul();
                        break;
                    case "/":
                        caculator = new Div();
                        break;             
                }
                return caculator;
            }
        }

        public interface ICaculator
        {
            //方法: 能够返回最终计算的结果
            double GetResult(double d1, double d2);
        }

        public class Add : ICaculator
        {
            public double GetResult(double d1, double d2)
            {
                return d1 + d2;
            }
        }

        public class Sub : ICaculator 
        {
            public double GetResult(double d1, double d2)
            {
                return d1 - d2;
            }
        }

        public class Mul : ICaculator
        {
            public double GetResult(double d1, double d2)
            {
                return d1 * d2;
            }
        }

        public class Div : ICaculator
        {
            public double GetResult(double d1, double d2)
            {
                return d1 / d2;
            }
        }
    }
}