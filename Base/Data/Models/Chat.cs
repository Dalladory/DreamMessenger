using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();


        [JsonIgnore]
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

        [JsonIgnore]
        public string LastMsgDateStr
        {
            get
            {
                //if (Messages == null || Messages.Count <= 0)
                //{
                //    return "";
                //}
                //return Messages.Last().SendDate.ToString();

                if (Messages.Count == 0) return "";
                if (Messages.Last().SendDate.ToString("d") == DateTime.Now.ToString("d"))
                {
                    return Messages.Last().SendDate.ToString("t");
                }
                else
                {
                    return Messages.Last().SendDate.ToString("g");
                }
            }

        }

    }
}
