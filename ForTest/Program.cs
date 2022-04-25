using Base.Data.Classes;
using Base.Data.Models;
using System;

namespace Program
{

    static class Program
    {
       
        static void Main()
        {
           

            //var users = UserService.GetUsers();
            //foreach (var user in users)
            //{
            //    Console.WriteLine(user.Name);
            //}

            Console.WriteLine(ChatAndMessageRepository.GetMessageById(2).Text); 


            //User user1 = new User
            //{
            //    Name = "Nikolai",
            //    Surname = "Burdiugh",
            //    Email = "burdiugh.bk@gmail.com",
            //    Login = "apexloIn23",
            //    Password = "rocketmaN123_tv",
            //};

            //User user2 = new User
            //{
            //    Name = "Daryna",
            //    Surname = "Kruk",
            //    Email = "kruk.daryna@gmail.com",
            //    Login = "kruk432Erf_sr",
            //    Password = "kruk_darynaPswd2003",
            //};

            //_userService.AddUser(user1);
            //_userService.AddUser(user2);

            //Chat chat = new Chat()
            //{
            //    CreatorId = 1,
            //    CompanionId = 2,
            //};

            //_chatAndMessageService.CreateChat(chat);

            //Message message = new Message()
            //{
            //    ChatId = 1,
            //    UserId = 2,
            //    Text = "Hello, how are you?",
            //    SendDate = DateTime.Now,
            //};
            //_chatAndMessageService.SendMessage(message);

            //Message message2 = new Message()
            //{
            //    ChatId = 1,
            //    UserId = 1,
            //    Text = "Hello, i am fine, thank you.",
            //    SendDate = DateTime.Now,
            //};
            //_chatAndMessageService.SendMessage(message2);

            //  Message message3 = new Message()
            //{
            //    ChatId = 1,
            //    UserId = 2,
            //    Text = "So that's it?",
            //    SendDate = DateTime.Now,
            //};
            //_chatAndMessageService.SendMessage(message3);


            //Console.WriteLine("Login: ");
            //string login = Console.ReadLine();
            //Console.WriteLine("Pass: ");
            //string pass = Console.ReadLine();

            //Console.WriteLine(_userService.IsValidCredentials(login, pass));







        }
    }


}