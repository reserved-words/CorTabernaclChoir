using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Services
{
    public interface IWorksService
    {
        WorksViewModel Get();
        void Save(Work model);
        Work GetForEdit(int id);
        void Delete(Work model);
    }
}