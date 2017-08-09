using System.ComponentModel.DataAnnotations;

namespace CorTabernaclChoir.Common.Models
{
    public class ContactEmail
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Address { get; set; }
    }
}
