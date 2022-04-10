using Base.Data.Classes;
using Base.Data.Models;
using System;

namespace Program
{

    static class Program
    {
       
        static void Main()
        {
            ChatAndMessageService _chatAndMessageService = new ChatAndMessageService(new ChatAndMessageRepository());
            UserService _userService = new UserService(new UserRepository());


            User user1 = new User
            {
                Name ="Nikolai",
                Surname = "Burdiugh",
                Email = "burdiugh.bk@gmail.com",
                Login ="apexloIn23",
                Password = "rocketmaN123_tv",
            };

            User user2 = new User
            {
                Name = "Daryna",
                Surname = "Kruk",
                Email = "kruk.daryna@gmail.com",
                Login = "kruk432Erf_sr",
                Password = "kruk_darynaPswd2003",
            };

            _userService.AddUser(user1);
            _userService.AddUser(user2);


        }
    }


}