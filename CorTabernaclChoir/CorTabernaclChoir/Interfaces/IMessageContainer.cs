using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CorTabernaclChoir.Messages;

namespace CorTabernaclChoir.Interfaces
{
    public interface IMessageContainer
    {
        bool ShowNewestOnTop { get; set; }
        bool ShowCloseButton { get; set; }
        List<Message> Messages { get; }
        void AddSaveSuccessMessage();
        void AddSaveErrorMessage();
    }
}