using Base.Data.Classes;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class UserService
    {

        readonly private UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetUsers()
        {
            try
            {
                return _userRepository.GetUsers();
            }
            catch (Exception)
            {
                // some trouble with getting data from db
            }
            return null;
        }

        public User GetUserById(int id)
        {
            try
            {
                return _userRepository.GetUserById(id);
            }
            catch (Exception)
            {
                // some trouble with getting data from db
            }
            return null;
        }

        public User GetUserByLogin(string login)
        {
            try
            {
                return _userRepository.GetUserByLogin(login);
            }
            catch (Exception)
            {
                // some trouble with getting data from db
            }
            return null ;
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
            try
            {
                _userRepository.RemoveUser(user);
            }
            catch (Exception)
            {
                // Some info to status bar or somewhere
            }
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

