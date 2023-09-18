namespace _1._0_Flyweight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //共享单车案例

            //创建享元工厂对象
            BikeFactory bikeFactory = new BikeFactory();

            FlyweightBike flyweightBike = bikeFactory.GetBike();
            flyweightBike.Ride("张三");

            FlyweightBike flyweightBike2 = bikeFactory.GetBike();
            flyweightBike2.Ride("李四");
            flyweightBike2.Back("李四");  //李四骑完车后将车归还

            FlyweightBike flyweightBike3 = bikeFactory.GetBike();
            flyweightBike3.Ride("王五");

            FlyweightBike flyweightBike4 = bikeFactory.GetBike();
            flyweightBike4.Ride("赵六");

        }

        public abstract class FlyweightBike
        {
            //内部状态: BikeID State: 0 -> 锁定中, 1 -> 骑行中
            //外部状态: 用户

            public string BikeID { get; set; }

            public int State { get; set; }

            public abstract void Ride(string userName);

            public abstract void Back(string userName);
        }

        public class YellowBike : FlyweightBike
        {
            public YellowBike(string id)
            {
                this.BikeID = id;
            }

            public override void Ride(string userName)
            {
                this.State = 1;
                Console.WriteLine("用户" + userName + "正在骑行ID是: " + this.BikeID + "的小黄车");
            }

            public override void Back(string userName)
            {
                this.State = 0;
                Console.WriteLine("用户" + userName + "归还了ID是: " + this.BikeID + "的小黄车");
            }
        }

        public class BikeFactory
        {
            List<FlyweightBike> bikePool = new List<FlyweightBike>();
            public BikeFactory()
            {
                //这里往池子里添加3辆车(数量可改)
                for(int i = 0;i < 3;i++)
                {
                    bikePool.Add(new YellowBike(i.ToString()));
                }
            }

            public FlyweightBike GetBike()
            {
                //如果池子里还有车就能拿出来, 如果没有车了则返回null
                for(int i = 0; i < bikePool.Count; i++)
                {
                    if (bikePool[i].State == 0)
                    {
                        return bikePool[i];
                    }
                }
                return null;
            }
        }
    }
}