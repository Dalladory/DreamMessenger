using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Managers
{
    public class UserManager
    {
        public static string SignIn(string data)
        {
            string[] arr = data.Split("|", 2, StringSplitOptions.RemoveEmptyEntries);
            User user = UserService.SignIn(arr[0], arr[1]);
            if (user == null) return "false|Login or password is wrong";
            return "true|" + user.Id.ToString();
        }
    }
}
