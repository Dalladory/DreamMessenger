using Base.Data.Classes;
using Base.Data.Models;

namespace ChatAndMessageService
{
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
           return _chatAndMessageRepository.GetMessageById(id);
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

        public void RemoveMessage(Message message) {

            _chatAndMessageRepository.RemoveMessages(message);

        }


        // Chat service's methods: 


        public Chat GetChatById(int id)
        {
            return _chatAndMessageRepository.GetChatById(id);
        }

        public void CreateChat(Chat chat)
        {
            try
            {
                _chatAndMessageRepository.CreateChat(chat);
            }
            catch (Exception ex)
            {
                // Some info about trouble
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
            _chatAndMessageRepository.RemoveChat(chat);
        }

    }
}