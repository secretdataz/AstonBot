using AstonBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AstonBot.WS;
using AstonBot.Commands.Impl;
using AstonBot.Commands.SubCommandImpl;
namespace AstonBot
{
    public class Aston
    {
        static List<Command> Commands = new List<Command>();
        static WSThreadWrapper WebsocketThread;

        private static string self;
        public static bool IgnoreGuest;
        public static Message.Role SelfRole = Message.Role.GUEST;
        public const string SELFMSG = "Hello. This is Aston bot for Mylive.in.th v0.1-ALPHA by secretdataz";

        public static string roomid;
        public static string roomowner;

        public Aston()
        {
            AddCommand();
            Console.Write("Enter Mylive room Id : ");
            roomid = Console.ReadLine();
            Console.Write("Enter user token (optional) : ");
            string token = System.Console.ReadLine();
            using (WebClient x = new WebClient())
            {
                string source = x.DownloadString("http://mylive.in.th/streams/" + roomid);
                string[] lines = source.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                for(int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if(Regex.IsMatch(line,"<title>"))
                    {
                        string SourceTitle = lines[i + 1];
                        Match TitleMatch = Regex.Match(SourceTitle, @"by (.*) :: MyLive.in.th");
                        if (TitleMatch.Success)
                        {
                            roomowner = TitleMatch.Groups[1].Value;
                        }
                        break;
                    }
                }
            }
            WebsocketThread = new WSThreadWrapper(roomid, token);
            while(true)
            {
                //
            }
        }

        public static void ParseMessage(dynamic data)
        {
            Message msg = GetMessage(data);
            if(msg.Sender == self)
            {
                return;
            }
            if(IgnoreGuest && !msg.UserRole.HasFlag(Message.Role.MEMBER))
            {
                return;
            }
            foreach(Command cmd in Commands)
            {
                if(msg.Msg.StartsWith(cmd.Trigger))
                {
                    cmd.Parse(msg);
                }
            }
            Console.WriteLine("[*] Message Received from " + msg.Sender + ": " + msg.Msg);
        }

        public static Message GetMessage(dynamic data)
        {
            string Sender = data.user;
            string Msg = data.msg.msg;
            string StrRole = data.role;
            Message.Role Role = ParseRole(StrRole);
            return new Message(Sender, Role, Msg, DateTime.Now);                 
        }

        public static void SendMessage(string Msg)
        {
            WebsocketThread.websocket.SendMessage(Msg);
        }

        public static void ParseAssign(dynamic data)
        {
            string StrRole = data.role;
            SelfRole = ParseRole(StrRole);
        }

        public static void ParseNotice(dynamic data)
        {
            string Msg = data.msg;
            Match UsernameMatch = Regex.Match(Msg,@"Welcome (.*)");
            if(UsernameMatch.Success)
            {
                self = UsernameMatch.Groups[1].Value;
                UpdateTitle();
            }
        }
        public void AddCommand()
        {
            var Asterisk = new AsteriskCommand();
            Asterisk.RegisterSubCommand(new SubCommandAbout());
            Asterisk.RegisterSubCommand(new SubCommandIgnoreGuest());
            Commands.Add(Asterisk);
            Commands.Add(new ChatFilter());
        }

        public static Message.Role ParseRole(string Role)
        {
            Message.Role EnumRole = Message.Role.GUEST;
            switch (Role)
            {
                case "m":
                    EnumRole = Message.Role.MEMBER; ;
                    break;
                case "o":
                    EnumRole = Message.Role.OWNER;
                    break;
                default:
                    EnumRole = Message.Role.GUEST;
                    break;
            }
            return EnumRole;
        }

        public static void UpdateTitle()
        {
            Console.Title = "Aston Bot :: " + self + " / Room " + roomowner + "(" + roomid + ")";
        }
    }
}
