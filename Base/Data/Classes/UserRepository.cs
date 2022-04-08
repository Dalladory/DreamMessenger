using Base.Data.Interfaces;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Classes
{
    public class UserRepository : IUserInterface
    {
        
        public void AddUser(User user)
        {
           AppDbContext context = new AppDbContext();
           context.Add(user);
           context.SaveChanges();
        }

        public void EditUser(User user)
        {
            AppDbContext context = new AppDbContext();
            context.Update(user);
            context.SaveChanges();
        }

        public List<User> GetUsers()
        {
            AppDbContext context = new AppDbContext();
            return context.Users.ToList();
        }
         public User GetUserById(uint id)
        {
            AppDbContext context = new AppDbContext();
            var users = context.Users.Where(b => b.Id == id).FirstOrDefault();
            return users;
        }
         public User GetUserByLogin(string login)
        {
            AppDbContext context = new AppDbContext();
            var user = context.Users.Where(b => b.Login == login).FirstOrDefault();
            return user;
        }

        public void RemoveUser(User user)
        {
            AppDbContext context = new AppDbContext();
            context.Remove(user);
            context.SaveChanges();
        }
    }
}
