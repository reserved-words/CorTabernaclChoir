using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Services
{
    public interface IGalleryService
    {
        GalleryViewModel Get();
        void Save(GalleryImage model);
        GalleryImage GetForEdit(int id);
        void Delete(GalleryImage model);
    }
}