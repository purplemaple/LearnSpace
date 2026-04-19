using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpClient
{
    internal class Client
    {
        Socket udpClient;

        public bool CreateSocket(IPEndPoint endPoint)
        {
            if(udpClient == null)
            {
                //1. 创建 Socket
                udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //如果客户端只管发送，不接收服务端的消息，则不需要写下面这些
                /*Task.Run(() =>
                {
                    while (true)
                    {
                        //2. 接收消息...
                    }
                });*/
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendMessage(string message, IPEndPoint endPoint)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient?.SendTo(data, endPoint);
        }
    }
}
