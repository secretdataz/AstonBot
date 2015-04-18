using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstonBot.Commands
{
    public interface Command
    {
        string Trigger
        {
            get;
        }

        bool RegisterSubCommand(SubCommand subCommand);

        SubCommand GetSubCommand(string CommandName);

        bool Parse(Message Msg);
    }
}
