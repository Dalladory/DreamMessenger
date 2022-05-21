using Base.Data.Classes;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Managers
{
    public class ChatManager
    {
        public static string CreateChat(string serializedChat, out string resultChat, out int companionId)
        {
            resultChat = "";
            companionId = -1;
            Chat chat;
            try
            {
                chat = JsonSerializer.Deserialize<Chat>(serializedChat);
                User companion = chat.Companion;
                chat.Companion = null;
                User creator = chat.Creator;
                chat.Creator = null;
                ChatAndMessageRepository.CreateChat(chat);
                chat.Companion = creator;
                chat.Creator = companion;
                resultChat = JsonSerializer.Serialize<Chat>(chat);
                companionId = chat.CompanionId;
            }
            catch (Exception ex)
            {
                return "false|" + ex.Message;
            }
            return "true|" + chat.Id;
        }

        

        public static string GetUserChats(int userId)
        {
            string chats = "";
            try
            {
                chats = JsonSerializer.Serialize<List<Chat>>(ChatAndMessageRepository.GetUserChats(userId));
            }
            catch (Exception ex)
            {
                return "false|" + ex.Message;
            }
            return "true|" + chats;
        }
    }
}
