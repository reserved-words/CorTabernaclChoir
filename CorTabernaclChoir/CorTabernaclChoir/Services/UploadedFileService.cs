using System.IO;
using System.Web;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Services
{
    public class UploadedFileService : IUploadedFileService
    {
        private const string GalleryImagesDirectory = "~/Images/Gallery";
        private const string PostImagesDirectory = "~/Images/Posts";

        public void SaveImage(HttpPostedFileBase file, ImageType imageType, int id, string fileExtension)
        {
            var filename = GetFilePath(imageType, id, fileExtension);

            file.SaveAs(filename);
        }

        public void DeleteImage(ImageType imageType, int id, string fileExtension)
        {
            var filename = GetFilePath(imageType, id, fileExtension);

            File.Delete(filename);
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

        private static string GetFilePath(ImageType imageType, int id, string fileExtension)
        {
            return HttpContext.Current.Server.MapPath(Path.Combine(GetDirectoryPath(imageType), $"{id}{fileExtension}"));
        }

        private static string GetDirectoryPath(ImageType imageType)
        {
            return imageType == ImageType.Gallery
                ? GalleryImagesDirectory
                : PostImagesDirectory;
        }
    }
}