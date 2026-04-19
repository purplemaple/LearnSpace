using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        /*
         * 本案例演示使用 UdpClient 类来简化 Udp 协议的 Socket 开发
         * 注：与 Tcp 协议不同的是，本案例只有 UdpClient 一个类，因为在 Udp 协议下不需要连接，服务端就是客户端，客户端就是服务端
         */
        static void Main(string[] args)
        {
            //1. 创建 UdpClient 对象    (客户端不需要绑定 IP 和端口号，发消息时会自带 IP，而端口号由操作系统随机分配(也可以指定))
            UdpClient udpClient = new UdpClient();

            while (true)
            {
                //2. 向服务端发送消息
                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);

                udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("172.18.138.212"), 12121));
            }
        }
    }
}