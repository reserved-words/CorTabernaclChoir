using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public class Home
    {
        public int Id { get; set; }

        [StringLength(1000)]
        [Required]
        [Display(Name = "Main Text (E)")]
        public string MainText_E { get; set; }

        [StringLength(1000)]
        [Required]
        [Display(Name = "Main Text (W)")]
        public string MainText_W { get; set; }

        [StringLength(100)]
        [Display(Name = "Musical Director")]
        public string Conductor { get; set; }

        [StringLength(100)]
        public string Accompanist { get; set; }
    }
}