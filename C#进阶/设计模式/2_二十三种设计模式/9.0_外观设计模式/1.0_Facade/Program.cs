namespace _1._0_Facade
{
    internal class Program
    {
        /*
         * 外观模式优点:
         *      1. 隐藏了系统的复杂性, 让客户端使用系统功能时变得简单
         *      2. 实现客户端和子系统间的解耦
         *      
         * 外观模式缺陷:
         *      1. 不符合开闭原则, 如果客户端需要使用更多功能, 不仅仅需要修改子系统, 也必须修改外观层
         */
        static void Main(string[] args)
        {
            /*
             * 如果不使用外观设计模式:
             */
            /*
            Police police = new Police();
            police.GetHuJi();

            Street street = new Street();
            street.GetHuKou();

            Hospital hospital = new Hospital();
            hospital.GetBorn();
            */

            DaTing daTing = new DaTing();
            daTing.ShowLicence();
        }

        public class DaTing
        {
            private Police Police = new Police();
            private Street street = new Street();
            private Hospital hospital = new Hospital();

            public void ShowLicence()
            {
                this.Police.GetHuJi();
                this.street.GetHuKou();
                this.hospital.GetBorn();
            }
        }

        public class Police
        {
            public void GetHuJi()
            {
                Console.WriteLine("开具户籍证明");
            }
        }

        public class Street
        {
            public void GetHuKou()
            {
                Console.WriteLine("开具户口证明");
            }
        }

        public class Hospital
        {
            public void GetBorn()
            {
                Console.WriteLine("开具出生证明");
            }
        }
    }
}