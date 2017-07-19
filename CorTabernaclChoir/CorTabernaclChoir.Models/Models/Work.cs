using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public class Work
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        public string Year { get; set; }

        [Required]
        [StringLength(100)]
        public string Composer { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }
    }
}