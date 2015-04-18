using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstonBot.Commands.SubCommandImpl
{
    public class SubCommandAbout : SubCommand
    {
        public string Name
        {
            get
            {
                return "about";
            }
        }

        public Message.Role AccessLevel
        {
            get
            {
                return Message.Role.GUEST;
            }
        }
        public bool Parse(Message msg, string[] args)
        {
            Aston.SendMessage("Hi! I'm Aston.");
            return true;
        }
    }
}
