using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            NewContactEmailAddress = new ContactEmail();
        }

        public List<ContactEmail> ContactEmailAddresses { get; set; }
        public ContactEmail NewContactEmailAddress { get; set; }
    }
}
