using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        /*
         * 本案例演示使用 TcpListener(用于服务器端) 和 TcpClient(用于客户端) 对 Socket 开发进行简化
         */
        static void Main(string[] args)
        {
            //1. TcpClient 创建时，就会自动和 Server 连接
            TcpClient client = new TcpClient("172.18.138.212", 10101);

            while (true)
            {
                //2. 向服务器发送数据
                string mesage = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(mesage);
                //写入数据流
                client.GetStream().Write(data, 0, data.Length);
            }
        }
    }
}