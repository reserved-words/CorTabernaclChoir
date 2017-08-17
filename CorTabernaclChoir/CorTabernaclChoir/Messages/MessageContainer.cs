using System;
using System.Collections.Generic;
using CorTabernaclChoir.Interfaces;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Messages
{
    [Serializable]
    public class MessageContainer : IMessageContainer
    {
        public bool ShowNewestOnTop { get; set; }
        public bool ShowCloseButton { get; set; }
        public List<Message> Messages { get; set; }

        public void AddSaveSuccessMessage()
        {
            var message = new Message
            {
                Title = MessageTitleSuccess,
                Text = MessageTextSaveSuccess,
                Type = MessageType.Success
            };
            Messages.Add(message);
        }

        public void AddSaveErrorMessage()
        {
            var message = new Message
            {
                Title = MessageTitleError,
                Text = MessageTextSaveError,
                Type = MessageType.Error
            };
            Messages.Add(message);
        }

        public MessageContainer()
        {
            Messages = new List<Message>();
            ShowNewestOnTop = false;
            ShowCloseButton = false;
        }
    }
}