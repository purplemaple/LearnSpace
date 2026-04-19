using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomTcpServer.Model
{
    /// <summary>
    /// Client 类，用来处理服务器与客户端的逻辑
    /// </summary>
    internal class Client
    {
        public Socket clientSocket;
        public Client(Socket socket)
        {
            this.clientSocket = socket;
        }
    }
}
