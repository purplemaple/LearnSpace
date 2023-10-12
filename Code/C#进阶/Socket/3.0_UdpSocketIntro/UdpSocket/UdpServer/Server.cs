using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    internal class Server
    {
        Socket udpServer;
        public bool CreateSocket(IPAddress address, int port)
        {
            if(udpServer == null)
            {
                //1. 创建 Socket
                udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //2. 绑定 IP 和端口号
                udpServer.Bind(new IPEndPoint(address, port));

                Task.Run(() =>
                {
                    while(true)
                    {
                        // 3. Udp 不需要监听、等待连接，直接接收数据
                        /*
                         * 0). ReceiveFrom() 方法会阻塞线程，只有当接收到一条数据时才会继续执行剩余代码
                         * 1). 下面的 ReceiveFrom() 参数列表需要 ref EndPoint，因此这里不能直接定义成子类 IPEndPoint
                         * 2). (IPAddress.Any, 0) 是因为暂时不知道连接过来的机器的 IP 和端口号，待会使用 ref 关键字，真正连上了在赋值
                         * 3). ref 关键字：可以使函数具有修改该参数的能力。此处等 ReceiveFrom() 方法真正执行，获取到连接终端的地址时，再将实际地址赋值回 remoteEndPoint
                         */
                        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] data = new byte[1024];
                        int length = udpServer.ReceiveFrom(data, ref remoteEndPoint);
                        string message = Encoding.UTF8.GetString(data, 0, length);

                        IPEndPoint? endPoint = remoteEndPoint as IPEndPoint;
                        ShowMessage(message, endPoint);
                    }
                });
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void ShowMessage(string message, IPEndPoint? endPoint)
        {
            Console.WriteLine("从：" + endPoint?.Address.ToString() + ": " + endPoint?.Port.ToString() + "收到如下信息：\r\n" + message);
        }
    }
}
