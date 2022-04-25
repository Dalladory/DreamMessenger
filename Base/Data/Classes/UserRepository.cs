using Base.Data.Interfaces;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Classes
{
    public class UserRepository
    {

        static public void AddUser(User user)
        {
            AppDbContext context = new AppDbContext();
            context.Add(user);
            context.SaveChanges();
        }

        static public void EditUser(User user)
        {
            AppDbContext context = new AppDbContext();
            context.Update(user);
            context.SaveChanges();
        }

        static public List<User> GetUsers()
        {
            AppDbContext context = new AppDbContext();
            return context.Users.ToList();
        }
        static public User GetUserById(int id)
        {
            AppDbContext context = new AppDbContext();
            var user = context.Users.Where(b => b.Id == id).FirstOrDefault();
            return user;
        }
        static public User GetUserByLogin(string login)
        {
            AppDbContext context = new AppDbContext();
            var user = context.Users.Where(b => b.Login == login).FirstOrDefault();
            return user;
        }

        static public void RemoveUser(User user)
        {
            AppDbContext context = new AppDbContext();
            context.Remove(user);
            context.SaveChanges();
        }

        static public bool IsValidCredentials(string login, string password)
        {
            AppDbContext context = new AppDbContext();
            var user = context.Users.Where(l => l.Login == login).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
