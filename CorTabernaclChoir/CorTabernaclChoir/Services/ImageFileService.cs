﻿using System;
using System.IO;
using System.Linq;
using System.Web;
using CorTabernaclChoir.Common.Exceptions;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Services
{
    public class ImageFileService : IImageFileService
    {
        private const int MaxImageMegaBytes = 1;
        private const int MaxImageBytes = 1000000 * MaxImageMegaBytes;
        private const string ValidationNoFileUploaded = "No image file was uploaded";
        private const string ValidationFileTooLarge = "The image uploaded was larger than {0}";
        private const string ValidationInvalidExtension = "The file uploaded must be an image file with extension {0}.";
        private const string TempImageFilenameFormat = "temp{0}.jpg";
        private const string ImageFilenameFormat = "{0}.jpg";

        private readonly string _maxImageBytesString = $"{MaxImageMegaBytes}MB";
        private readonly string[] _validContentTypes = { "image/jpeg", "image/pjpeg", "image/png" };
        
        public string Save(HttpPostedFileBase file, string imagesFolder, string[] validExtensions, bool throwIfNoFileUploaded)
        {
            if (!FileUploaded(file))
            {
                if (throwIfNoFileUploaded)
                {
                    throw new ValidationException(ValidationNoFileUploaded);
                }

                return string.Empty;
            }
            
            string result;
            if (!ValidateFile(file, validExtensions, out result) || !SaveTempFile(file, imagesFolder, out result))
            {
                throw new ValidationException(result);
            }

            return result;
        }

        private static bool FileUploaded(HttpPostedFileBase file)
        {
            return file != null && file.ContentLength > 0;
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

        private bool SaveTempFile(HttpPostedFileBase file, string imagesFolder, out string result)
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

        public bool ValidateFile(HttpPostedFileBase file, string[] validExtensions, out string error)
        {
            error = string.Empty;
            
            if (file == null ||file.ContentLength <= 0)
            {
                return true;
            }

            if (file.ContentLength > MaxImageBytes)
            {
                error = string.Format(ValidationFileTooLarge, _maxImageBytesString);
                return false;
            }

            if (!_validContentTypes.Contains(file.ContentType) || !validExtensions.Contains(Path.GetExtension(file.FileName.ToLower())))
            {
                error = string.Format(ValidationInvalidExtension, string.Join(" or ", validExtensions));
                return false;
            }

            return true;
        }
    }
}