using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            IPEndPoint endPoint = new(IPAddress.Parse("172.18.138.212"), 9090);

            client.CreateSocket(endPoint);
            while (true)
            {
                string message = Console.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    client.SendMessage(message, endPoint);
                }
            }
        }
    }
}