using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class GalleryViewModel
    {
        public List<Image> Images { get; set; }
    }

    public class Image
    {
        public int Id { get; set; }
        public string Caption { get; set; }
    }
}