using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatRoomTcpClient_2.Model
{
    internal class ClientModel : BindableBase
    {
        Socket? TcpClientSocket;
        public string ClientInfo { get; set; }

        public bool CreatSocket()
        {
            if (TcpClientSocket == null)
            {
                //1. 创建客户端 Socket
                TcpClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //2. 建立连接
                TcpClientSocket.Connect(new IPEndPoint(IPAddress.Parse(IP), Convert.ToInt32(Port)));

                /*
                 * LocalEndPoint 与 RemoteEndPoint：
                 * LocalEndPoint：返回当前终端的终节点
                 * RemoteEndPoint：返回远程连接终端的终节点
                 * 
                 * 例如，在客户端使用 RemoteEndPoint 时，返回的是服务端，在服务端使用时，返回的是客户端
                 */
                ClientInfo = TcpClientSocket.LocalEndPoint.ToString();

                Task.Run(() =>
                {
                    while (true)
                    {
                        //3. 接收消息
                        byte[] receiveBuffer = new byte[1024];
                        int length = TcpClientSocket.Receive(receiveBuffer);
                        string message = Encoding.UTF8.GetString(receiveBuffer, 0, length);
                        //将接收到的消息发送到 UI 线程
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            if (ChatTextBox != null)
                            {
                                ChatTextBox += Environment.NewLine;
                            }
                            ChatTextBox += message;
                        });
                    }
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendMessage(string message)
        {
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);
            TcpClientSocket?.Send(byteMessage);
        }


        /// <summary>
        /// IP地址
        /// </summary>
        private string _IP = "172.18.138.212";
        public string IP
        {
            get => _IP;
            set
            {
                SetProperty(ref _IP, value);
            }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        private string? _Port = "8989";
        public string? Port
        {
            get => _Port;
            set
            {
                SetProperty(ref _Port, value);
            }
        }

        /// <summary>
        /// 聊天框
        /// </summary>
        private string? _ChatTextBox;
        public string? ChatTextBox
        {
            get => _ChatTextBox;
            set
            {
                SetProperty(ref _ChatTextBox, value);
            }
        }

        /// <summary>
        /// 发送框
        /// </summary>
        private string? _SendTextBox;
        public string? SendTextBox
        {
            get => _SendTextBox;
            set
            {
                SetProperty(ref _SendTextBox, value);
            }
        }
    }
}
