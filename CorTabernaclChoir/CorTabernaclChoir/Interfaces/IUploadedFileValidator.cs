using System.Web;

namespace CorTabernaclChoir.Interfaces
{
    public interface IUploadedFileValidator
    {
        bool IsFileUploaded(HttpPostedFileBase file);

        bool ValidateFile(HttpPostedFileBase file, string[] validExtensions, int? maxFileSizeKB, out string error);

        bool ValidateSquareImage(HttpPostedFileBase file, string[] validExtensions, int minWidth, int maxWidth, int? maxFileSizeKB, out string error);
    }
}
