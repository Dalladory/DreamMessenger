using Base.Data.Classes;
using Base.Data.Models;
using System.Diagnostics;

public class ChatAndMessageService
    {
       

        // Message service's methods: 

        public Message GetMessageById(int id)
        {
            try
            {
                return ChatAndMessageRepository.GetMessageById(id);
            }
            catch (Exception)
            {
                // something went wrong with getting data from db
            }
            return null;
        }

        public void SendMessage(Message message)
        {
            try
            {
            ChatAndMessageRepository.SendMessage(message);
            }
            catch (Exception ex)
            {
                // Some info about trouble
            }
        }

        public void EditMessage(Message message)
        {
            try
            {
            ChatAndMessageRepository.EditMessage(message);
            }
            catch (Exception ex)
            {
                // Some info about trouble
            }
        }

        public void RemoveMessage(Message message)
        {
            try
            {
            ChatAndMessageRepository.RemoveMessages(message);
            }
            catch (Exception)
            {
                // Some info about trouble
            }

        }


        // Chat service's methods: 


        public Chat GetChatById(int id)
        {
            try
            {
                return ChatAndMessageRepository.GetChatById(id);

            }
            catch (Exception)
            {
                // something went wrong with getting data from db
            }
            return null;
        }

        public void CreateChat(Chat chat)
        {
            try
            {
            ChatAndMessageRepository.CreateChat(chat);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void EditChat(Chat chat)
        {
            try
            {
            ChatAndMessageRepository.EditChat(chat);
            }
            catch (Exception ex)
            {
                // Some info about trouble
            }
        }

        public void RemoveChat(Chat chat)
        {
            try
            {
            ChatAndMessageRepository.RemoveChat(chat);
            }
            catch (Exception)
            {
                // Some info about trouble
            }
        }

    }
