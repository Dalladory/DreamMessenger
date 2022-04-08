using Base.Data.Classes;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService
{
    public class UserService
    {

        readonly private UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public User GetUsetById(uint id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByLogin(string login)
        {
            return _userRepository.GetUserByLogin(login);
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public void RemoveUser(User user)
        {
            _userRepository.RemoveUser(user);
        }

        public void EditUser(User user)
        {
            _userRepository.EditUser(user);
        }


    }
}
