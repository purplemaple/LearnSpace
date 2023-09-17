namespace DependenceInversionPrincipleOptimize2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //不同的人开不同的车

            /* 1. 通过接口注入
            Student student = new Student();
            student.Drive(new Benz());*/

            /* 2. 通过构造方法注入
            Student student = new Student(new Benz());
            student.Drive();*/

            /* 3. 通过Set方法注入 */
            Student student = new Student();
            student.SetCar(new Benz());
            student.Drive();
        }

        public interface ICar
        {
            void Run();
        }

        public interface IDriver
        {
            /* 1. 通过接口注入
            //在接口或者类中，将要注入的服务对象，以参数的形式直接注入，称之为接口注入
            void Drive(ICar car);*/

            /* 3. 通过Set注入 */
            void SetCar(ICar car);
            void Drive();
        }

        public class Benz : ICar
        {
            public void Run()
            {
                Console.WriteLine("奔驰在奔跑");
            }
        }

        public class Student : IDriver
        {
            /* 1. 通过接口注入
            public void Drive(ICar car)
            {
                car.Run();
            }*/

            /* 2. 通过构造方法注入
            private ICar _car;
            public Student(ICar car)
            {
                this._car = car;
            }
            public void Drive()
            {
                this._car.Run();
            }*/

            private ICar _car;
            public void SetCar(ICar car)
            {
                this._car = car;
            }

            public void Drive()
            {
                this._car.Run();
            }
        }


    }
}