using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Base.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int CreatorId { get; set; }
        //public User Creator { get; set; }
        public int CompanionId { get; set; }
        //public User Companion { get; set; }

        public string Text { get; set; }
        public DateTime SendDate { get; set; }

        [JsonIgnore]
        public string SendDateStr
        {
            get
            {
                if (SendDate.ToString("d") == DateTime.Now.ToString("d"))
                {
                    return SendDate.ToString("t");
                }
                else
                {
                    return SendDate.ToString("g");
                }
            }

        }
    }
}
