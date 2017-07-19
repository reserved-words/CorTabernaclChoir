using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            ContactForm = new ContactFormViewModel();
        }

        public string MainText { get; set; }
        public ContactFormViewModel ContactForm { get; set; }
    }
}