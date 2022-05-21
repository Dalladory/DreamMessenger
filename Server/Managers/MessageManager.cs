using Base.Data.Classes;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Managers
{
    public class MessageManager
    {
        public static string AddMessage(string serializedMessage, out int companionId)
        {
            companionId = -1;
            try
            {
                Message message = JsonSerializer.Deserialize<Message>(serializedMessage);
                ChatAndMessageRepository.SendMessage(message);
                companionId = message.CompanionId;
            }
            catch (Exception ex)
            {
                return "false|" + ex.Message;
            }
            return "true";
        }

        
    }
}
