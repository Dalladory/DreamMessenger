using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Interfaces
{
    public interface IChatAndMessageInterface
    {
        // Message interface's methods:
        void SendMessage(Message message);
        Message GetMessageById(int id);
        void RemoveMessages(Message message);
        void EditMessage(Message message);

        // Chat interface's methods:

        void CreateChat(Chat chat);
        void EditChat(Chat chat);
        Chat GetChatById(int id);
        void RemoveChat(Chat chat);

    }
}
