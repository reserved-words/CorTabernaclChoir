using System.Web;
using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Interfaces
{
    public enum ImageType { Post, Gallery }

    public interface IUploadedFileService
    {
        void SaveImage(HttpPostedFileBase file, ImageType imageType, int id, string fileExtension);
        void DeleteImage(ImageType imageType, int id, string fileExtension);
        ImageFile Convert(HttpPostedFileBase file);

    }
}
