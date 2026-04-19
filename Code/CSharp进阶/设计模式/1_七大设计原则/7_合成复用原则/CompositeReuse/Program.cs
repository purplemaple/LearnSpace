namespace CompositeReuse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * 使用对象组合的原则: has 与 is 的区别
             * 
             * A is base : 使用继承 (如: 学生是人，所以学生继承人类)
             * 
             * base has A : 使用对象组合 (如: 大雁有翅膀, 所以大雁类和翅膀类组合)
             */

            Car car = new QYCar();
            car.Run(new Green());
            car.Run(new Red());
        }

        public interface IColor
        {
            string ShowColor();
        }

        public class Green : IColor
        {
            public string ShowColor()
            {
                return "绿色";
            }
        }

        public class Red : IColor
        {
            public string ShowColor()
            {
                return "红色";
            }
        }

        public abstract class Car
        {
            //接口依赖
            public abstract void Run(IColor color);
        }

        public class QYCar : Car    // QY : 汽油
        {
            public override void Run(IColor color)
            {
                Console.WriteLine("这辆" + color.ShowColor() + "的汽油车正在行驶~~");
            }
        }

        public class DCar : Car     // D : 电
        {
            public override void Run(IColor color)
            {
                Console.WriteLine("这辆" + color.ShowColor() + "的电动车正在行驶~~");
            }
        }
    }
}