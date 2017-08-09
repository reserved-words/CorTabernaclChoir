using System;
using System.Collections.Generic;
using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Common.Services
{
    public interface IEmailService
    {
        List<ContactEmail> GetAddresses();
        void AddAddress(ContactEmail email);
        void RemoveAddress(int id);
    }
}
