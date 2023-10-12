using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        /*
         * 本案例演示使用 UdpClient 类来简化 Udp 协议的 Socket 开发
         * 注：与 Tcp 协议不同的是，本案例只有 UdpClient 一个类，因为在 Udp 协议下不需要连接，服务端就是客户端，客户端就是服务端
         */
        static void Main(string[] args)
        {
            //1. 创建 UdpClient 对象并绑定地址
            UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse("172.18.138.212"), 12121));

            while (true)
            {
                //2. 接收数据
                /*
                 * 类似于案例 3.0_UdpSocketIntro，先创建一个未指定地址的 IPEndPoint，等到真正接收到消息时使用关键字 ref 来赋值
                 */
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
                //接收消息并使用 ref 将地址赋值回 point
                byte[] data = udpClient.Receive(ref point);
                string message = Encoding.UTF8.GetString(data);

                Console.WriteLine("收到了消息：" + message);
            }
        }
    }
}