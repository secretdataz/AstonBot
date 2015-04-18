using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AstonBot.WS
{
    public class WSThreadWrapper
    {
        public Thread thread
        {
            get;
            set;
        }
        public AstonWebSocket websocket
        {
            get;
            set;
        }

        public WSThreadWrapper(string roomid, string token)
        {
            thread = new Thread(() => this.websocket = new AstonWebSocket(roomid, token));
            thread.Start();
        }
    }
}
