using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Models
{
    public class ErrorsLog
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime SendDate { get; set; }

        public ErrorsLog(string message)
        {
            ErrorMessage = message;
            SendDate = DateTime.Now;
        }
        public ErrorsLog()
        {

        }
    }
}
