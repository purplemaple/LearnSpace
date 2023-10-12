using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.CreateSocket(IPAddress.Parse("172.18.138.212"), 9090);
            Console.ReadKey();
        }
    }
}