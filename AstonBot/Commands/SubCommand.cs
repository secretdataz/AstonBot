using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstonBot.Commands
{
    public interface SubCommand
    {
        string Name
        {
            get;
        }

        Message.Role AccessLevel
        {
            get;
        }

        bool Parse(Message Msg, string[] args);
    }
}
