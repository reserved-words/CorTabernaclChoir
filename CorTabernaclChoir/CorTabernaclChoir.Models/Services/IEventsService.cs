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
        EventsViewModel GetAll(int page);
        EventViewModel GetById(int id);
        EditEventViewModel GetForEdit(int id);
        int Save(EditEventViewModel model);
        int SaveImage(int eventId, string fileExtension);
        void Delete(Event model);
        void DeleteImage(int imageId);
    }
}
