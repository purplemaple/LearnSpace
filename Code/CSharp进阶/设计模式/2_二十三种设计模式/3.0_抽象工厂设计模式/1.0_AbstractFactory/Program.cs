using System.Reflection;

namespace _1_AbstractFactory
{
    internal class Program
    {
        /*
         * 抽象工厂模式与工厂模式的区别: 抽象工厂里的工厂类里能生产多个类的对象
         * 如何抉择使用哪种模式: 如果需要在同一工厂下生产类似的、同品牌、同型号的产品等, 就选择抽象工厂, 因为能避免工厂类这个层级数量爆炸
         */
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("请输入需要的品牌:");
                string brand = Console.ReadLine();

                ReflectionFactory rf = new ReflectionFactory();
                AbstractFactory abstractFactory = rf.GetFac(brand);

                IKeyboard keyboard = abstractFactory.GetKeyboard();
                IMouse mouse = abstractFactory.GetMouse();

                keyboard.ShowKeyboardBrand();
                mouse.ShowMouseBrand();
            }

        }
        public class OperToFactory : Attribute
        {
            public string Oper { get; }     //因为值是我们直接写好, 贴到类上的, 不需要提供set索引器
            public OperToFactory(string s)
            {
                this.Oper = s;
            }
        }

        public class ReflectionFactory
        {
            Dictionary<string, AbstractFactory> dic = new Dictionary<string, AbstractFactory>();

            public ReflectionFactory()                  //调用无参构造
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                foreach(var item in assembly.GetTypes())
                {
                    if(typeof(AbstractFactory).IsAssignableFrom(item) && !item.IsInterface)
                    {
                        OperToFactory otf = item.GetCustomAttribute<OperToFactory>();
                        if(otf.Oper != null)
                        {
                            dic[otf.Oper] = Activator.CreateInstance(item) as AbstractFactory;
                        }
                    }
                }
            }

            /*
             * 调用有参构造, args 中填入对应的参数列表
             * 注: 当 args 为 null 时, 会转而调用无参构造
             */
            public ReflectionFactory(object[] args)     
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                foreach (var item in assembly.GetTypes())
                {
                    if (typeof(AbstractFactory).IsAssignableFrom(item) && !item.IsInterface)
                    {
                        OperToFactory otf = item.GetCustomAttribute<OperToFactory>();
                        if (otf.Oper != null)
                        {
                            dic[otf.Oper] = Activator.CreateInstance(item, args) as AbstractFactory;
                        }
                    }
                }
            }

            public AbstractFactory GetFac(string s)
            {
                if (dic.ContainsKey(s))
                {
                    return dic[s];
                }
                return null;
            }
        }

        #region 抽象工厂接口及不同品牌的工厂实现类
        /// <summary>
        /// 抽象工厂接口
        /// </summary>
        public interface AbstractFactory
        {
            IKeyboard GetKeyboard();
            IMouse GetMouse();
        }

        [OperToFactory("Dell")]
        public class DellFactory : AbstractFactory
        {
            public IKeyboard GetKeyboard()
            {
                return new DellKeyboard();
            }

            public IMouse GetMouse()
            {
                return new DellMouse();
            }
        }

        [OperToFactory("HP")]
        public class HPFactory : AbstractFactory
        {
            public IKeyboard GetKeyboard()
            {
                return new HPKeyboard();
            }

            public IMouse GetMouse()
            {
                return new HPMouse();
            }
        }

        [OperToFactory("Lenovo")]
        public class LenovoFactory : AbstractFactory
        {
            public IKeyboard GetKeyboard()
            {
                return new LenovoKeyboard();
            }

            public IMouse GetMouse()
            {
                return new LenovoMouse();
            }
        }
        #endregion

        //创建不同品牌(Dell、HP、Lenovo)的鼠标、键盘、主机
        #region 产品接口及不同品牌的实现类
        public interface IKeyboard
        {
            void ShowKeyboardBrand();
        }

        public class DellKeyboard : IKeyboard
        {
            public void ShowKeyboardBrand()
            {
                Console.WriteLine("我是戴尔品牌的键盘");
            }
        }

        public class HPKeyboard : IKeyboard
        {
            public void ShowKeyboardBrand()
            {
                Console.WriteLine("我是惠普品牌的键盘");
            }
        }

        public class LenovoKeyboard : IKeyboard
        {
            public void ShowKeyboardBrand()
            {
                Console.WriteLine("我是Lenovo品牌的键盘");
            }
        }

        public interface IMouse
        {
            void ShowMouseBrand();
        }

        public class DellMouse : IMouse
        {
            public void ShowMouseBrand()
            {
                Console.WriteLine("我是戴尔品牌的鼠标");
            }
        }

        public class HPMouse : IMouse
        {
            public void ShowMouseBrand()
            {
                Console.WriteLine("我是惠普品牌的鼠标");
            }
        }

        public class LenovoMouse : IMouse
        {
            public void ShowMouseBrand()
            {
                Console.WriteLine("我是Lenovo品牌的鼠标");
            }
        }
        #endregion

        
    }
}