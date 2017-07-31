using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class SocialMediaViewModel
    {
        public int Id { get; set; }
        [Display(Name = "URL")]
        public string Url { get; set; }
        public string Name { get; set; }
        public int? ImageFileId { get; set; }
    }
}
