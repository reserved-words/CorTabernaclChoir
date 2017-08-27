using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public class PostImage
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string FileExtension { get; set; }

        public virtual Post Post { get; set; }
    }
}