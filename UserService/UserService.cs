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
            try
            {
                _userRepository.AddUser(user);
            }
            catch (Exception ex)
            {
                // Some info to status bar or somewhere
            }
           
        }

        public void RemoveUser(User user)
        {
            _userRepository.RemoveUser(user);
        }

        public void EditUser(User user)
        {
            try
            {
                _userRepository.EditUser(user);

            }
            catch (Exception)
            {
                // Some info to status bar or somewhere
            }
        }


    }
}
