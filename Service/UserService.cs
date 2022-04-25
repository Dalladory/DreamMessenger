using Base.Data.Classes;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UserService
{

    static public List<User> GetUsers()
    {
        try
        {
            return UserRepository.GetUsers();
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
            return UserRepository.GetUserById(id);
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
            return UserRepository.GetUserByLogin(login);
        }
        catch (Exception)
        {
            // some trouble with getting data from db
        }
        return null;
    }

    public void AddUser(User user)
    {
        try
        {
            UserRepository.AddUser(user);
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
            UserRepository.RemoveUser(user);
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
            UserRepository.EditUser(user);

        }
        catch (Exception)
        {
            // Some info to status bar or somewhere
        }
    }

    public bool IsValidCredentials(string login, string password)
    {
        return UserRepository.IsValidCredentials(login, password);
    }

}

