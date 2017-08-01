using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Models
{
    public class ImageFile
    {
        public int Id { get; set; }
        public byte[] File { get; set; }
        public string ContentType { get; set; }
    }
}
