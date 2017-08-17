using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Messages
{
    [Serializable]
    public class Message
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public MessageType Type { get; set; }
        public bool IsSticky { get; set; }
    }
}