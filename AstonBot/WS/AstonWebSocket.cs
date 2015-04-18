using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AstonBot.WS
{
    public class AstonWebSocket
    {
        private WebSocket websocketClient;
        private string url;
        private string protocol;
        private WebSocketVersion version;
        private string roomid;
        private string token;

        public AstonWebSocket(string roomid, string token)
        {
            this.roomid = roomid;
            this.token = token;
            Init();
            Start();
        }

        public void Init()
        {
            this.url = "ws://mylive.in.th:9004/home/websocket/";
            this.protocol = "";
            this.version = WebSocketVersion.Rfc6455;
            websocketClient = new WebSocket(this.url, this.protocol, this.version);
            websocketClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocketClient_Error);
            websocketClient.Opened += new EventHandler(websocketClient_Opened);
            websocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocketClient_MessageReceived);
            Console.WriteLine("[-] WebSocket initiated.");
        }
        public void Start()
        {
            websocketClient.Open();
            Console.WriteLine("[-] WebSocket starting.");
        }

        public void Stop()
        {
            websocketClient.Close();
            Console.WriteLine("[-] WebSocket closing.");
        }
        
        public void SendMessage(string Msg)
        {
            var json = JsonConvert.SerializeObject(new { cmd = "msg", msg = Msg },
                new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.None });
            websocketClient.Send(json);
        }

        public void DeleteMessage(string ChatId)
        {
            var json = JsonConvert.SerializeObject(new { cmd = "delete", cid = ChatId },
                new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.None });
            websocketClient.Send(json);
        }

        public void Ban(string uid, string gid, string name)
        {
            var json = JsonConvert.SerializeObject(new { cmd = "ban", userid = uid, guestid = gid, username = name },
                new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.None });
            websocketClient.Send(json);
        }
        private void websocketClient_Opened(object sender, EventArgs e)
        {
            var json = JsonConvert.SerializeObject(new { cmd = "auth", hashkey = token, roomname = roomid },
            new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.None });
            websocketClient.Send(json);
            System.Console.WriteLine("[-] WebSocket started.");
            SendMessage(Aston.SELFMSG);
        }

        private void websocketClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            System.Console.WriteLine(e.Exception.GetType() + ": " + e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);

            if (e.Exception.InnerException != null)
            {
                System.Console.WriteLine(e.Exception.InnerException.GetType());
            }

            return;
        }

        private void websocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
            dynamic json = JValue.Parse(e.Message);
            string type = json.type;
            switch(type)
            {
                case "chat":
                    Aston.ParseMessage(json);
                    break;
                case "notice":
                    Aston.ParseNotice(json);
                    break;
                case "assign":
                    Aston.ParseAssign(json);
                    break;
                default:
                    Console.WriteLine("Unknow message type : " + e.Message);
                    break;
            }
        }
    }
}
