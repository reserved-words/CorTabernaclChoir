using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Models
{
    public class ContactEmail
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Address { get; set; }
    }
}
