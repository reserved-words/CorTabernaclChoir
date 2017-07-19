using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Services
{
    public interface IEventsService
    {
        EventsViewModel Get(int page);
        Event GetForEdit(int id);
        void Save(Event model);
        void Delete(Event model);
    }
}
