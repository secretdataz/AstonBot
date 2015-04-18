using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstonBot.Commands.Impl
{
    public class ChatFilter : CommandImpl
    {
        public override string Trigger
        {
            get
            {
                return String.Empty;
            }
        }
        
        public override bool Parse(Message msg)
        {
            switch(msg.Msg)
            {
                case "ดีจ้า":
                    Aston.SendMessage("สวัสดีจ้า ;)");
                    break;
                case "!john":
                    Aston.SendMessage("ไม่ใช่จอห์น!");
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
