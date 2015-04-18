using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstonBot.Commands.Impl
{
    public abstract class CommandImpl : Command
    {
        private List<SubCommand> SubCommandList = new List<SubCommand>();
        public abstract string Trigger
        {
            get;
        }

        public bool RegisterSubCommand(SubCommand Command)
        {
            SubCommand sc = GetSubCommand(Command.Name);
            if (sc != null) return false;
            SubCommandList.Add(Command);
            return true;
        }
        
        public SubCommand GetSubCommand(string Name)
        {
            try
            {
                return SubCommandList.Single(x => x.Name.ToLower() == Name.ToLower());
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public virtual bool Parse(Message Msg)
        {
            List<string> args = new List<string>();
            args.AddRange(Msg.Msg.Split(' '));
            string cmd = args.First().Substring(1);
            SubCommand sc = GetSubCommand(cmd);
            if(sc != null)
            {
                if(!Msg.UserRole.HasFlag(sc.AccessLevel))
                {
                    return false;
                }
                args.RemoveAt(0);
                return sc.Parse(Msg, args.ToArray());
            }
            return false;
        }
    }
}
