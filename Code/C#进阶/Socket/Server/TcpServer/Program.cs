using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1. 创建Socket
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            /* 2. 绑定 Ip 和端口号 172.18.138.212
             * EndPoint 是一个抽象类
             * IPEndPoint 是对 Ip 和端口做了一层封装的类
             */
            IPAddress ipAddress = new IPAddress(new byte[] { 172, 18, 138, 212 });
            EndPoint point = new IPEndPoint(ipAddress, 8181);
            tcpServer.Bind(point);  //向操作系统申请一个可用的ip和端口号，用来通信

            //3. 开始监听（等待客户端连接）
            tcpServer.Listen(100);  //参数是允许的最大连接数
            Console.WriteLine("开始监听...");

            //暂停当前线程，直到有一个客户端连接过来后进行之后的代码
            Socket clientScoket = tcpServer.Accept();
            Console.WriteLine(clientScoket.LocalEndPoint + " 已连接 " + DateTime.Now.ToString());

            //使用返回的 Socket 与客户端通信
            string message = "hello, 欢迎你的连接";
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientScoket.Send(data);
            Console.WriteLine("向客户端发送了一条数据");

            //接收来自客户端的消息
            byte[] data2 = new byte[1024];
            int length = clientScoket.Receive(data2);
            string message2 = Encoding.UTF8.GetString(data2, 0, length);
            Console.WriteLine("收到从客户端发来的消息：" + message2);
        }
    }
}