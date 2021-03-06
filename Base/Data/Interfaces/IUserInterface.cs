using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Interfaces
{
    public  interface IUserInterface
    {
        void AddUser(User user);
        List<User> GetUsers();
        void RemoveUser(User user);
        void EditUser(User user);
        User GetUserById(int id);
        User GetUserByLogin(string login);
        bool IsValidCredentials(string login, string password);

    }
}
