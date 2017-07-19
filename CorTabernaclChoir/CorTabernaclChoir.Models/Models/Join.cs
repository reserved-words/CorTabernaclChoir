using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public class Join
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
        [Display(Name = "Rehearsal Times (E)")]
        public string RehearsalTimes_E { get; set; }

        [StringLength(100)]
        [Display(Name = "Location (E)")]
        public string Location_E { get; set; }

        [StringLength(100)]
        [Display(Name = "Auditions (E)")]
        public string Auditions_E { get; set; }

        [StringLength(100)]
        [Display(Name = "Concerts (E)")]
        public string Concerts_E { get; set; }

        [StringLength(100)]
        [Display(Name = "Subscriptions (E)")]
        public string Subscriptions_E { get; set; }

        [StringLength(100)]
        [Display(Name = "Rehearsal Times (W)")]
        public string RehearsalTimes_W { get; set; }

        [StringLength(100)]
        [Display(Name = "Location (W)")]
        public string Location_W { get; set; }

        [StringLength(100)]
        [Display(Name = "Auditions (W)")]
        public string Auditions_W { get; set; }

        [StringLength(100)]
        [Display(Name = "Concerts (W)")]
        public string Concerts_W { get; set; }

        [StringLength(100)]
        [Display(Name = "Subscriptions (W)")]
        public string Subscriptions_W { get; set; }
    }
}