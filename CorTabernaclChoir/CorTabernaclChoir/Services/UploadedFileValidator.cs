using System.IO;
using System.Linq;
using System.Web;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Services
{
    public class UploadedFileValidator : IUploadedFileValidator
    {
        private const int MaxImageMegaBytes = 1;
        private const int MaxImageBytes = 1000000 * MaxImageMegaBytes;
        private const string ValidationFileTooLargeMB = "The image uploaded was larger than {0}MB";
        private const string ValidationFileTooLargeKB = "The image uploaded was larger than {0}KB";
        private const string ValidationInvalidExtension = "The file uploaded must be an image file with extension {0}.";
        private const string ErrorMessageImageNotSquare = "The uploaded image must have the same width and height";
        private const string ErrorMessageImageDimensions = "The uploaded image must have height and width between {0}px and {1}px";

        private readonly string[] _validContentTypes = { "image/jpeg", "image/pjpeg", "image/png" };

        public bool IsFileUploaded(HttpPostedFileBase file)
        {
            return file != null && file.ContentLength > 0;
        }

        public string GetFileExtension(HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName);
        }

        public bool ValidateFile(HttpPostedFileBase file, string[] validExtensions, int? maxFileSizeKB, out string error)
        {
            error = string.Empty;
            
            if (file == null ||file.ContentLength <= 0)
            {
                return true;
            }

            if (!ValidateFileSize(file, maxFileSizeKB, out error))
                return false;


            if (!_validContentTypes.Contains(file.ContentType) || !validExtensions.Contains(Path.GetExtension(file.FileName.ToLower())))
            {
                error = string.Format(ValidationInvalidExtension, string.Join(" or ", validExtensions));
                return false;
            }

            return true;
        }

        public bool ValidateSquareImage(HttpPostedFileBase file, string[] validExtensions, int minWidth, int maxWidth, int? maxFileSizeKB, out string error)
        {
            return ValidateFile(file, validExtensions, maxFileSizeKB, out error) 
                && ValidateSquareImageDimensions(file, minWidth, maxWidth, out error);
        }

        private static bool ValidateFileSize(HttpPostedFileBase file, int? maxFileSizeKB, out string error)
        {
            error = string.Empty;

            if (maxFileSizeKB.HasValue)
            {
                if (file.ContentLength > maxFileSizeKB * 1000)
                {
                    error = string.Format(ValidationFileTooLargeKB, maxFileSizeKB);
                    return false;
                }
            }
            else if (file.ContentLength > MaxImageBytes)
            {
                error = string.Format(ValidationFileTooLargeMB, MaxImageMegaBytes);
                return false;
            }

            return true;
        }

        private static bool ValidateSquareImageDimensions(HttpPostedFileBase file, int minWidth, int maxWidth, out string error)
        {
            error = string.Empty;
            
            var image = System.Drawing.Image.FromStream(file.InputStream);

            if (image.Height != image.Width)
            {
                error = ErrorMessageImageNotSquare;
                return false;
            }

            if (image.Width > maxWidth || image.Width < minWidth)
            {
                error = string.Format(ErrorMessageImageDimensions, minWidth, maxWidth);
                return false;
            }

            file.InputStream.Position = 0;

            return true;
        }
    }
}