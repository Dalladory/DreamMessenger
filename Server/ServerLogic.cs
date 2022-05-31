using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ServerLogic
    {
        public static Dictionary<int, Socket> authorizedСlients = new Dictionary<int, Socket>();
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void Run()
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, 8080));
            socket.Listen();
            while (true)
            {
                Socket client = socket.Accept();
                if(client != null)
                {
                    Console.WriteLine("New client connected... Dictionary length: " + (authorizedСlients.Count + 1));
                    Task.Run(() => new Client(client));
                }
            }
        }
    }
}