using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstonBot.Commands.SubCommandImpl
{
    public class SubCommandIgnoreGuest : SubCommand
    {
        public string Name
        {
            get
            {
                return "ignoreguest";
            }
        }

        public Message.Role AccessLevel
        {
            get
            {
                return Message.Role.MODERATOR;
            }
        }
        public bool Parse(Message msg, string[] args)
        {
            Aston.IgnoreGuest = !Aston.IgnoreGuest;
            Aston.SendMessage("Toggled guest ignore mode to " + Aston.IgnoreGuest.ToString());
            return true;
        }
    }
}
