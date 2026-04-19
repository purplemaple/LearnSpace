using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChatRoomTcpServer.Model
{
    internal class ServerModel : BindableBase
    {
        Socket? TcpServerSocket;
        List<Client> clientList = new();
        /*
         * 注：互斥锁
         */
        Mutex mutex = new Mutex(false);

        public bool CreatSocket()
        {
            if (TcpServerSocket == null)
            {
                //1. 创建服务器 Socket
                TcpServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //2. 与本地端口绑定
                TcpServerSocket.Bind(new IPEndPoint(IPAddress.Parse(IP), Convert.ToInt32(Port)));
                //3. 设置监听最大限制
                TcpServerSocket.Listen(100);
                /* 4. 接收客户端连接请求
                 * 没有收到连接请求时会等待，会造成线程阻塞，因此另开线程进行连接
                 */
                Task.Run(() =>
                {
                    while (true)
                    {
                        Socket clientSocket = TcpServerSocket.Accept();
                        Client client = new Client(clientSocket);

                        string clientInfo = clientSocket.RemoteEndPoint.ToString();

                        /*
                         * 注：Dispatcher.BeginInvoke 与 Dispatcher.Invoke 的区别：
                         * 1. 同异步区别：
                         *      BeginInvoke 是异步的，不需要等待UI线程完成，可以直接继续下面的代码
                         *      Invoke 是同步的，需要等待UI线程
                         * 2. 返回值区别：
                         *      BeginInvoke 返回一个DispatcherOperation对象，它可以用来与UI操作进行交互，例如取消或等待。
                         *      Invoke 返回一个object对象，它是UI操作的返回值
                         * 3. 优先级区别：
                         *      BeginInvoke 和 Invoke 都可以指定一个 DispatcherPriority 来决定该操作在消息队列中的优先级。
                         *      但是如果使用 Invoke 时指定了一个低于当前子线程的优先级，那么可能会造成死锁
                         * 
                         * 知识点补充：
                         * BeginInvoke 和 Invoke 都是把 UI 操作封装成一个DispatcherOperation对象，然后放入消息队列中，由主线程从队列中取出并执行
                         * 1. UI线程会按照优先级的顺序来取出并执行，优先级由 DispatcherPriority 枚举确定，值越大优先级越高，不指定时默认为 DispatcherPriority.Normal = 9
                         * 2. 指定 DispatcherPriority：示例：Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, () =>{})     (DataBind = 8)
                         * 3. 线程相关知识：
                         *      如果线程正在执行一个优先级为5的任务，没有等待或休眠，此时又来了一个优先级为8的任务，那么UI线程会先前者执行完再去执行后者。这是因为线程只有在执行完一个任务后才会检查消息队列中是否有新的任务
                         *      遇到等待或休眠比如 Thread.Sleep(0) ，则线程会去寻找已就绪且优先级最高的任务来执行。若刚才执行休眠的线程的优先级很低，那可能要很久才能执行
                         */
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            if (ChatTextBox1 != null)
                            {
                                ChatTextBox1 += Environment.NewLine;
                            }
                            ChatTextBox1 += clientInfo + " 已连接.";
                        });
                        
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                        /*
                         * 注：因为当前线程操作了集合，下面的新线程中又操作了同一集合，因此要加互斥锁
                         */
                        lock (mutex)
                        {
                            clientList.Add(client);
                        }

                        /* 接收与发送客户端消息
                         * 同样，未收到消息时会阻塞线程, 因此需要另开线程
                         */
                        Task.Run(() =>
                        {
                            while (true)
                            {
                                //如果断开连接则跳出
                                if (!clientSocket.Connected)
                                {
                                    break;
                                }
                                //5. 接收消息
                                byte[] receiveBuffer = new byte[1024];
                                int length = clientSocket.Receive(receiveBuffer);
                                string message = Encoding.UTF8.GetString(receiveBuffer, 0, length);
                                //将接收到的消息传递给 UI 线程
                                Application.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (ChatTextBox2 != null)
                                    {
                                        ChatTextBox2 += Environment.NewLine;
                                    }
                                    ChatTextBox2 += message;
                                });
                                
                                /* 6. 发送消息，广播给所有客户端
                                 * 当前应用是聊天室，每个终端发送的消息，所有人都能看见，暂不支持一对一聊天窗口
                                 * 注：因为上面的线程操作了集合，此线程中又操作了同一集合，因此要加互斥锁
                                 */
                                lock (mutex)
                                {
                                    clientList.ForEach(client => client.clientSocket.Send(receiveBuffer));
                                }
                            }
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
        /// 聊天框1
        /// </summary>
        private string? _ChatTextBox1;
        public string? ChatTextBox1
        {
            get => _ChatTextBox1;
            set
            {
                SetProperty(ref _ChatTextBox1, value);
            }
        }

        /// <summary>
        /// 聊天框2
        /// </summary>
        private string? _ChatTextBox2;
        public string? ChatTextBox2
        {
            get => _ChatTextBox2;
            set
            {
                SetProperty(ref _ChatTextBox2, value);
            }
        }
    }
}
