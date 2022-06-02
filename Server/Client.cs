using Base.Data.Models;
using Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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
                            int userId;
                            result = UserManager.SignIn(arr[1], out userId);
                            Task.Run(() => AddMeToAuthorizedClients(userId));
                            break;
                        }
                    case "AddUser":
                        {
                            result = UserManager.AddUser(arr[1]);
                            Task.Run(() => AddMeToAuthorizedClients(result));
                            break;
                        }
                    case "AddMessage":
                        {
                            int companionId;
                            result = MessageManager.AddMessage(arr[1], out companionId);
                            if (result.StartsWith("true|"))
                            {
                                Task.Run(() => SendMessageToCompanion(arr[1], companionId));
                            }
                            break;
                        }
                    case "CreateChat":
                        {
                            int companionId;
                            string resultChat;
                            result = ChatManager.CreateChat(arr[1], out resultChat, out companionId);
                            if (result.StartsWith("true|"))
                            {
                                Task.Run(() => SendChatToCompanion(resultChat, companionId));
                            }
                            break;
                        }
                    case "SearchUsers":
                        {
                            result = UserManager.SearchUsers(arr[1]);
                            break;
                        }
                    case "GetUserChats":
                        {
                            result = ChatManager.GetUserChats(int.Parse(arr[1]));
                            break;
                        }
                    default:
                        break;
                }
                socket.Send(Encoding.UTF8.GetBytes(result, 0, result.Length), SocketFlags.None);
            }
        }

        private void SendMessageToCompanion(string serializedMessage, int companionId)
        {
            try
            {
                Socket companionSocket = ServerLogic.authorizedСlients[companionId];
                if(companionSocket != null)
                {
                    companionSocket.Send(Encoding.UTF8.GetBytes("AddMessage|" + serializedMessage));
                    Console.WriteLine("Message send to companion: " + "AddMessage|" + serializedMessage);
                }
                else
                {
                    Console.WriteLine("Companion not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send a message to the companion: " + ex.Message);
            }
        }

        private void SendChatToCompanion(string serializedChat, int companionId)
        {
            try
            {
                Socket companionSocket = ServerLogic.authorizedСlients[companionId];
                if (companionSocket != null)
                {
                    companionSocket.Send(Encoding.UTF8.GetBytes("AddChat|" + serializedChat));
                    Console.WriteLine("Message send to companion: " + "AddChat|" + serializedChat);
                }
                else
                {
                    Console.WriteLine("Companion not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send a chat to the companion: " + ex.Message);
            }
        }

        private void AddMeToAuthorizedClients(string result)
        {
            if (result.StartsWith("true"))
            {
                ServerLogic.authorizedСlients[int.Parse(result.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)[1])] = socket;
                //ServerLogic.authorizedСlients.Add(int.Parse(result.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)[1]), socket);
            }
            else
            {
                Console.WriteLine("Add authorized client error: " + result);
            }
        }

        private void AddMeToAuthorizedClients(int id)
        {
            if (id >= 0)
            {
                ServerLogic.authorizedСlients[id] = socket;
                //ServerLogic.authorizedСlients.Add(int.Parse(result.Split("|", 2, StringSplitOptions.RemoveEmptyEntries)[1]), socket);
            }
            else
            {
                Console.WriteLine("Add authorized client error: " + id);
            }
        }

    }
}
