using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ResponseParser
    {
        public static bool Parse(string response, out string message)
        {
            if(response == null)
            {
                message = "";
                return false;
            }
            string[] arr = response.Split("|", 2, StringSplitOptions.RemoveEmptyEntries);
            if(arr[1] != null)
            {
                message = arr[1];
            }
            else
            {
                message = "";
            }
            if(response.StartsWith("true|"))
            {
                return true;
            }
            else if(response.StartsWith("false|"))
            {
                return false;
            }
            else
            {
                message = "Unknown error";
                return false;
            }
        }
    }
}
