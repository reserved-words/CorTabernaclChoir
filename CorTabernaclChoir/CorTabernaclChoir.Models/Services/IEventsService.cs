using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Common.Services
{
    public interface IEventsService
    {
        EventsViewModel GetAll();
        EventViewModel GetById(int id);
        EditEventViewModel GetForEdit(int id);
        int Save(EditEventViewModel model);
        int SaveImage(int eventId, string fileExtension);
        void Delete(Event model);
        void DeleteImage(int imageId);
    }
}
