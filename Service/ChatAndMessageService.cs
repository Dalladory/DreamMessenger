using Base.Data.Classes;
using Base.Data.Models;
using System.Diagnostics;

public class ChatAndMessageService
    {
        readonly ChatAndMessageRepository _chatAndMessageRepository;

        public ChatAndMessageService(ChatAndMessageRepository chatAndMessageRepository)
        {
            _chatAndMessageRepository = chatAndMessageRepository;
        }


        // Message service's methods: 

        public Message GetMessageById(int id)
        {
            try
            {
                return _chatAndMessageRepository.GetMessageById(id);
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
                _chatAndMessageRepository.SendMessage(message);
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
                _chatAndMessageRepository.EditMessage(message);
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
                _chatAndMessageRepository.RemoveMessages(message);
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
                return _chatAndMessageRepository.GetChatById(id);

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
                _chatAndMessageRepository.CreateChat(chat);
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
                _chatAndMessageRepository.EditChat(chat);
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
                _chatAndMessageRepository.RemoveChat(chat);
            }
            catch (Exception)
            {
                // Some info about trouble
            }
        }

    }
