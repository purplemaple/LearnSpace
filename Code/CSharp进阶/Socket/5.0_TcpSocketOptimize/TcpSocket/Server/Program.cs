using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    internal class Program
    {
        /*
         * 本案例演示使用 TcpListener(用于服务器端) 和 TcpClient(用于客户端) 来简化 Tcp 协议的 Socket 开发
         */
        static void Main(string[] args)
        {
            //1. TcpListener 对 Socket 进行了封装，这个类里面自己会创建 Socket 对象
            TcpListener listener = new TcpListener(IPAddress.Parse("172.18.138.212"), 10101);

            //2. 开始监听
            listener.Start();

            //3. 等待客户端连接
            TcpClient client = listener.AcceptTcpClient();

            //4. 取得客户端发送过来的数据
            byte[] data = new byte[1024];

            while (true)
            {
                /*
                 * 获取数据流，并从中读取数据
                 * Read(byte[] buffer, int offset, int count)：
                 *      buffer：读取数据后存放的数组
                 *      offset: 从数组的指定下标处开始存放
                 *      count：单次读取的最大量，不足时读取剩余全部
                 */
                int length = client.GetStream().Read(data, 0, data.Length);
                string message = Encoding.UTF8.GetString(data, 0, length);
                Console.WriteLine("收到了消息：" + message);
            }
        }
    }
}