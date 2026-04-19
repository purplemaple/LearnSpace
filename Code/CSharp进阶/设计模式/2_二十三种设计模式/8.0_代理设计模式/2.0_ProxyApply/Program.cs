namespace _2._0_ProxyApply
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //创建真实干活类和代理对象
            IOrder order = new ProxyOrder(new RealOrder("笔记本电脑", 100, "张三"));

            //代理对象以 "李四" 的身份进行修改操作  -->  操作失败
            order.SetOrderCount(ConsoleApp1200, "李四");

            //代理对象以 "张三" 的身份进行修改操作  -->  操作成功
            order.SetOrderCount(300, "张三");
            
            Console.WriteLine(order.GetOrderCount());
        }

        /// <summary>
        /// 封装了干活对象和代理对象的公用接口
        /// </summary>
        public interface IOrder
        {
            /// <summary>
            /// 获取订单中产品的名称
            /// </summary>
            /// <returns></returns>
            string GetProductName();

            /// <summary>
            /// 设置订单中产品的名称
            /// </summary>
            /// <param name="productName">订单中产品的名称</param>
            /// <param name="user">操作员</param>
            void SetProductName(string productName, string user);

            /// <summary>
            /// 获取订单中的产品的数量
            /// </summary>
            /// <returns></returns>
            int GetOrderCount();

            /// <summary>
            /// 设置订单中的产品数量
            /// </summary>
            /// <param name="count">产品数量</param>
            /// <param name="user">操作员</param>
            void SetOrderCount(int count, string user);

            /// <summary>
            /// 获取创建订单的用户
            /// </summary>
            /// <returns></returns>
            string GetOrderUser();

            /// <summary>
            /// 设置创建订单的用户
            /// </summary>
            /// <param name="orderUserName">创建订单的用户</param>
            /// <param name="user">操作员</param>
            void SetOrderUser(string orderUserName, string user);
        }


        /// <summary>
        /// 真实干活类
        /// </summary>
        public class RealOrder : IOrder
        {
            public string ProductName { get; set; }
            public int ProductCount { get; set; }
            public string OrderUser { get; set; }

            public RealOrder(string productName, int productCount, string orderUser)
            {
                this.ProductName = productName;
                this.ProductCount = productCount;
                this.OrderUser = orderUser;
            }


            public string GetOrderUser()
            {
                return this.OrderUser;
            }

            public int GetOrderCount()
            {
                return this.ProductCount;
            }

            public string GetProductName()
            {
                return this.GetProductName();
            }

            public void SetOrderUser(string orderUserName, string user)
            {
                this.OrderUser= orderUserName;
            }

            public void SetProductName(string productName, string user)
            {
                this.ProductName= productName;
            }

            public void SetOrderCount(int count, string user)
            {
                this.ProductCount = count;
            }
        }

        /// <summary>
        /// 代理类, 除了调用干活类外, 还要对修改操作设置访问权限
        /// </summary>
        public class ProxyOrder : IOrder
        {
            //封装一个实体对象的引用
            private RealOrder realOrder;
            public ProxyOrder(RealOrder realOrder)
            {
                this.realOrder = realOrder;
            }

            public string GetOrderUser()
            {
                return this.realOrder.GetOrderUser();
            }

            public int GetOrderCount()
            {
                return this.realOrder.GetOrderCount();
            }

            public string GetProductName()
            {
                return this.realOrder.GetProductName();
            }


            public void SetOrderUser(string orderUserName, string user)
            {
                //对权限进行判断, 有权限才可以访问, 没权限的不行
                if(user != null && user.Equals(this.GetOrderUser()))
                {
                    this.realOrder.SetOrderUser(orderUserName, user);
                    Console.WriteLine("成功修改数据!");
                }
                else
                {
                    Console.WriteLine("您无权修改订单数据!");
                }
            }

            public void SetOrderCount(int count, string user)
            {
                //对权限进行判断, 有权限才可以访问, 没权限的不行
                if (user != null && user.Equals(this.GetOrderUser()))
                {
                    this.realOrder.SetOrderCount(count, user);
                    Console.WriteLine("成功修改数据!");
                }
                else
                {
                    Console.WriteLine("您无权修改订单数据!");
                }
            }

            public void SetProductName(string productName, string user)
            {
                //对权限进行判断, 有权限才可以访问, 没权限的不行
                if (user != null && user.Equals(this.GetOrderUser()))
                {
                    this.realOrder.SetProductName(productName, user);
                    Console.WriteLine("成功修改数据!");
                }
                else
                {
                    Console.WriteLine("您无权修改订单数据!");
                }
            }
        }
    }
}