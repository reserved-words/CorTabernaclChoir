using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class JoinViewModel
    {
        public JoinViewModel()
        {
            ContactForm = new ContactFormViewModel();
        }

        public string RehearsalTimes { get; set; }
        public string Location { get; set; }
        public string Auditions { get; set; }
        public string Concerts { get; set; }
        public string Subscriptions { get; set; }
        public string MainText { get; set; }
        public ContactFormViewModel ContactForm { get; set; }
    }
}