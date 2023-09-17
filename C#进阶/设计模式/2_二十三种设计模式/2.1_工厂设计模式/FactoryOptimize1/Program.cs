using System.Reflection;

namespace FactoryOptimize1
{
    internal class Program
    {
        /*
         *  使用反射法解决掉switch语句
         * 
         */
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
             * 原本只有一个工厂, 现在有几种业务就有几种工厂
             * 第一步: 拿到对应的工厂对象
             * 第二步: 通过工厂对象拿到业务对象
             * 第三步: 通过业务对象完成业务
             */

            /*
            ICalFactory calFactory = null;
            switch (oper)
            {
                default:
                case "+":
                    calFactory = new AddFactory();
                    break;
                case "-":
                    calFactory = new SubFactory();
                    break;
                case "*":
                    calFactory = new MulFactory();
                    break;
                case "/":
                    calFactory = new DivFactory();
                    break;
            }
            ICaculator caculator = calFactory.GetCalculator();
            Console.WriteLine(caculator.GetResult(d1, d2));
            */

            ReflectionFactory reflectionFactory = new ReflectionFactory();
            ICalFactory calFactory = reflectionFactory.GetFac(oper);
            ICaculator caculator = calFactory.GetCalculator();

            Console.WriteLine(caculator.GetResult(d1, d2));
        }



        /*
         * 1. 使用 Attribute 给代码贴狗皮膏药
         * Attribute: 特性
         */
        public class OperToFactory : Attribute
        {
            public string Oper { get; }     //因为值是我们直接写好, 贴到类上的, 不需要提供set索引器
            public OperToFactory(string s)
            {
                this.Oper = s;
            }
        }

        //2. 程序运行后拿到这段描述关系, 并且返回相应的对象
        public class ReflectionFactory
        {
            //3. 字典对照表, 用于装载输入的字符和工厂实现类, 将字符与实现类一一对应
            Dictionary<string, ICalFactory> dic = new Dictionary<string, ICalFactory>();

            public ReflectionFactory()
            {
                //4. 利用反射拿到当前正在运行的程序集
                Assembly assembly = Assembly.GetExecutingAssembly();

                //AddFactory    SubFactory    MulFactory    DivFactory
                //5. 遍历当前程序集中所有的类
                foreach (var item in assembly.GetTypes())       
                {
                    /*
                     * 6. 
                     * typeof(ICalFactory).IsAssignableFrom(item):  当 item 是 ICalFactory 极其子类(实现类)时, 返回 true , 否则 false
                     * 
                     * !item.IsInterface:   这里只想拿到 ICalFactory 的实现类, 并不想拿到他自己这个接口本身
                     * 
                     */
                    if (typeof(ICalFactory).IsAssignableFrom(item) && !item.IsInterface)
                    {
                        /*
                         * 7.
                         * 拿类上方注明的狗皮膏药(Attribute)
                         * 这里拿到的 .Oper 是我们自定义的属性, 其实就是 Attribute 括号里的字符串, 如:
                         * [OperToFactory("+")]: 拿到 +
                         * [OperToFactory("*")]: 拿到 *
                         */
                        OperToFactory otf = item.GetCustomAttribute<OperToFactory>();
                        if(otf.Oper != null)
                        {
                            /*
                             * 8. 
                             * Activator.CreateInstance(item) as ICalFactory: 根据 Attribute 创建 ICalFactory 的不同实现类
                             * 然后装入 Dictionary 里, 完成字符串与实现类一一对应
                             */
                            dic[otf.Oper] = Activator.CreateInstance(item) as ICalFactory;
                        }
                    }
                }
            }

            /*
             * 9. Dictionary 中已将字符串与实现类一一对应, 封装方法返回即可, 不存在的返回 null
             */
            public ICalFactory GetFac(string s)
            {
                if (dic.ContainsKey(s))
                {
                    return dic[s];
                }
                return null;
            }

        }

        public interface ICalFactory
        {
            ICaculator GetCalculator();
        }

        //1.5 类上贴上狗皮膏药(Attribute)
        [OperToFactory("+")]
        public class AddFactory : ICalFactory
        {
            public ICaculator GetCalculator()
            {
                return new Add();
            }
        }

        //1.5 类上贴上狗皮膏药(Attribute)
        [OperToFactory("-")]
        public class SubFactory : ICalFactory
        {
            public ICaculator GetCalculator()
            {
                return new Sub();
            }
        }

        //1.5 类上贴上狗皮膏药(Attribute)
        [OperToFactory("*")]
        public class MulFactory : ICalFactory
        {
            public ICaculator GetCalculator()
            {
                return new Mul();
            }
        }

        //1.5 类上贴上狗皮膏药(Attribute)
        [OperToFactory("/")]
        public class DivFactory : ICalFactory
        {
            public ICaculator GetCalculator()
            {
                return new Div();
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