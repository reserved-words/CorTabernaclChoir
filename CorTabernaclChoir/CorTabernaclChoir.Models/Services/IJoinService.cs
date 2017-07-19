using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Services
{
    public interface IJoinService
    {
        JoinViewModel Get();
        Join GetForEdit();
        void Save(Join model);
    }
}
