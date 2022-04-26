using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServerLogic
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void Run()
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, 8080));
            socket.Listen();
            while (true)
            {
                Socket client = socket.Accept();
                Console.WriteLine("New client connected");
                Task.Run(() => new Client(client));
            }
        }
    }
}
