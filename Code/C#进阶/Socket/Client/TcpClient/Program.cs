using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1. 创建Socket
            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            /* 2. 发起建立连接的请求
             * Parse()：可以将一个 ip 地址的字符串转成 IPAdress 对象
             */
            IPAddress ipAddress = IPAddress.Parse("172.18.138.212");
            EndPoint point = new IPEndPoint(ipAddress, 8181);
            tcpClient.Connect(point);

            byte[] data = new byte[1024];
            /*
             * Receive()：给定一个字节数组，用于接收服务器端发来的数据
             * 返回值 length：表示接收了多少字节的数据
             */
            int length = tcpClient.Receive(data);

            //将字节数组转化为字符串
            string message = Encoding.UTF8.GetString(data, 0, length);

            Console.WriteLine(message);

            //向服务器端发送消息
            string message2 = Console.ReadLine();   //读取用户输入，发送到服务端
            tcpClient.Send(Encoding.UTF8.GetBytes(message2));
        }
    }
}