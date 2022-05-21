using Base.Data.Classes;
using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Managers
{
    public class UserManager
    {
        public static string SignIn(string data, out int userId)
        {
            userId = -1;
            try
            {
                string[] arr = data.Split("|", 2, StringSplitOptions.RemoveEmptyEntries);
                User user = UserRepository.SignIn(arr[0], arr[1]);
                if (user == null) return "false|Login or password is wrong";
                userId = user.Id;
                return "true|" + JsonSerializer.Serialize<User>(user);
            }
            catch (Exception ex)
            {
                return "false|" + ex.Message;
            }
        }

        public static string AddUser(string jsonSerializedUser)
        {
            try
            {
                User user = JsonSerializer.Deserialize<User>(jsonSerializedUser);
                UserRepository.AddUser(user);
                return "true|" + user.Id;
            }
            catch (Exception ex)
            {
                return "false|" + ex.Message;
            }
        }

        public static string SearchUsers(string text)
        {
            string users;
            try
            {
                users = JsonSerializer.Serialize<List<User>>(UserRepository.SearchUsers(text));
            }
            catch (Exception ex)
            {
                return "false|" + ex.Message;
            }
            return "true|" + users;
        }
    }
}
