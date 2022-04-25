using Base.Data.Interfaces;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Classes
{
    public class ChatAndMessageRepository
    {
        static public void CreateChat(Chat chat)
        {
            AppDbContext context = new AppDbContext();
            context.Add(chat);
            context.SaveChanges();
        }

        static public void EditChat(Chat chat)
        {
            AppDbContext context = new AppDbContext();
            context.Update(chat);
            context.SaveChanges();
        }

        static public void EditMessage(Message message)
        {
            AppDbContext context = new AppDbContext();
            context.Update(message);
            context.SaveChanges();
        }

        static public Chat GetChatById(int id)
        {
            AppDbContext context = new AppDbContext();
            return context.Chats.Where(u => u.Id == id).FirstOrDefault();
        }

        static public Message GetMessageById(int id)
        {
            AppDbContext context = new AppDbContext();
            return context.Messages.Where(u => u.Id == id).FirstOrDefault(); ;
        }

        static public void RemoveChat(Chat chat)
        {
            AppDbContext context = new AppDbContext();
            context.Remove(chat);
            context.SaveChanges();
        }

        static public void RemoveMessages(Message message)
        {
            AppDbContext context = new AppDbContext();
            context.Remove(message);
            context.SaveChanges();
        }

        static public void SendMessage(Message message)
        {
            AppDbContext context = new AppDbContext();
            context.Add(message);
            context.SaveChanges();
        }
    }
}
