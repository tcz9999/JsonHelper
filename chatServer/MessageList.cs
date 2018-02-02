using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace chatServer
{
    public static class MessageList
    {
        static Dictionary<string, List<Message>> _list = new Dictionary<string, List<Message>>();
        
        public static string sendMsg(string from, string to, string msg)
        {
            List<Message> m;
            if (!_list.ContainsKey(to))
            {
                m = new List<Message>();
                _list.Add(to, m);
            }
            else
            {
                m = _list[to];
            }
            m.Add(new Message(from,msg));

            return "";
        }
        public static List<Message> getMsg(string who)
        {
            List<Message> m =null;
            if (_list.ContainsKey(who))
            {
                m = _list[who];
                _list.Remove(who);
            }
            return m;
        }
        public static void clear()
        {
            _list.Clear();
        }
    }

    [DataContract]
    public class Message
    {
        public Message(string from ,string msg)
        {
            From = from;
            Msg = msg;
        }
        [DataMember]
        string From;
        [DataMember]
        string Msg;
    }
}