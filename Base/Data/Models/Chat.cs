using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public int CompanionId { get; set; }
        public User Companion { get; set; }


        public List<Message> Messages { get; set; }

        public string LastMsg
        {
            get
            { 
                if(Messages == null || Messages.Count <= 0)
                {
                    return "";
                }
                return Messages.Last().Text;
            }
        }

    }
}
