using System.Web;
using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Interfaces
{
    public interface IImageFileService
    {
        string Save(HttpPostedFileBase file, string imagesFolder, bool throwIfNoFileUploaded);

        void Move(string currentLocation, string newDirectory, int id);

        void Delete(string path);

        void Delete(string directory, int id);

        void Delete(string directory, string filename);

        ImageFile Convert(HttpPostedFileBase file);

        bool ValidateFile(HttpPostedFileBase file, out string error);
    }
}
