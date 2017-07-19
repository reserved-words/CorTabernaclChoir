using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public class GalleryImage
    {
        public int Id { get; set; }
        
        [StringLength(200)]
        [Required]
        [Display(Name = "Caption (E)")]
        public string Caption_E { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Caption (W)")]
        public string Caption_W { get; set; }

        [StringLength(4, MinimumLength = 4)]
        public string Year { get; set; }
    }
}