using System;
using System.IO;
using System.Web;
using CorTabernaclChoir.Common.Exceptions;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Services
{
    public class UploadedFileService : IUploadedFileService
    {
        private const string TempImageFilenameFormat = "temp{0}.jpg";
        private const string ImageFilenameFormat = "{0}.jpg";
        
        public string Save(HttpPostedFileBase file, string imagesFolder, string[] validExtensions, bool throwIfNoFileUploaded)
        {
            string result;

            if (!SaveTempFile(file, imagesFolder, out result))
            {
                throw new ValidationException(result);
            }

            return result;
        }

        public void Move(string currentLocation, string newDirectory, int id)
        {
            var path = Path.Combine(newDirectory, string.Format(ImageFilenameFormat, id));

            if (File.Exists(path))
            {
                Delete(path);
            }

            File.Move(currentLocation, path);
        }

        public void Delete(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return;

            File.Delete(path);
        }

        public void Delete(string directory, int id)
        {
            var filename = Path.Combine(directory, string.Format(ImageFilenameFormat, id));

            Delete(filename);
        }

        public void Delete(string directory, string filename)
        {
            Delete(Path.Combine(directory, filename));
        }

        public ImageFile Convert(HttpPostedFileBase file)
        {
            using (var ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                return new ImageFile
                {
                    File = ms.ToArray(),
                    ContentType = file.ContentType
                };
            }
        }

        private static bool SaveTempFile(HttpPostedFileBase file, string imagesFolder, out string result)
        {
            var tempFilename = string.Format(TempImageFilenameFormat, DateTime.Now.ToString("yyyyMMddHHmmss"));
            result = Path.Combine(imagesFolder, tempFilename);

            try
            {
                file.SaveAs(result);
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }

            return true;
        }
    }
}