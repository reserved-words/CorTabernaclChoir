﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public class About
    {
        public int Id { get; set; }

        [StringLength(4000)]
        [Required]
        [Display(Name = "About the Choir (E)")]
        public string AboutChoir_E { get; set; }

        [StringLength(4000)]
        [Required]
        [Display(Name = "About the Choir (W)")]
        public string AboutChoir_W { get; set; }

        [StringLength(2000)]
        [Display(Name = "About the Musical Director (E)")]
        public string AboutMusicalDirector_E { get; set; }

        [StringLength(2000)]
        [Display(Name = "About the Musical Director (W)")]
        public string AboutMusicalDirector_W { get; set; }

        [StringLength(2000)]
        [Display(Name = "About the Accompanist (E)")]
        public string AboutAccompanist_E { get; set; }

        [StringLength(2000)]
        [Display(Name = "About the Accompanist (W)")]
        public string AboutAccompanist_W { get; set; }
    }
}