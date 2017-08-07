using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Services
{
    public interface IEmailService
    {
        List<string> GetForwardingAddresses();
        void AddForwardingAddress(string email);
        void RemoveForwardingAddress(string email);
    }
}
