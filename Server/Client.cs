using Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Client
    {
        Socket socket;
        int userId;

        public Client(Socket socket)
        {
            this.socket = socket;
            Receive();
        }

        private void Receive()
        {
            int bytesCount;

            while (true)
            {
                byte[] buffer = new byte[1024];
                bytesCount = socket.Receive(buffer);

                string[] arr = Encoding.UTF8.GetString(buffer, 0, bytesCount).Split("|", 2, StringSplitOptions.RemoveEmptyEntries);
                string result = "";
                Console.WriteLine("Client send text: " + arr[0] + " || " + arr[1]);
                switch (arr[0])
                {
                    case "SignIn":
                        {
                            result = UserManager.SignIn(arr[1]);
                            break;
                        }
                    case "AddUser":
                        {
                            result = UserManager.AddUser(arr[1]);
                            break;
                        }
                    default:
                        break;
                }
                socket.Send(Encoding.UTF8.GetBytes(result, 0, result.Length), SocketFlags.None);
            }
        }

    }
}
