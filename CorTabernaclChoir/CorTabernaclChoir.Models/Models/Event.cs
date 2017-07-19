using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public class Event : Post
    {
        [Required]
        public DateTime Date { get; set; }

        [StringLength(200)]
        [Required]
        public string Venue_E { get; set; }

        [StringLength(200)]
        [Required]
        public string Venue_W { get; set; }

        [StringLength(200)]
        [Required]
        public string Address_E { get; set; }

        [StringLength(200)]
        [Required]
        public string Address_W { get; set; }
    }
}