using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstonBot
{
    public class Message
    {
        public Message(string sender, Role role, string Msg, DateTime time)
        {
            this.Sender = sender;
            this.UserRole = role;
            this.Msg = Msg;
            this.Time = time;
        }
        public string Sender;
        public Role UserRole;
        public string Msg;
        public DateTime Time;
        
        [Flags]
        public enum Role
        {
            GUEST = 0,
            MEMBER = 1,
            MODERATOR = 3,
            OWNER = 7
        }
    }
}
