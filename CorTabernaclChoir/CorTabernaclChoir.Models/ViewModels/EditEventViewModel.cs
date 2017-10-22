using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class EditEventViewModel
    {
        public EditEventViewModel()
        {
            PostImages = new List<PostImageViewModel>();
        }

        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Title (E)")]
        public string Title_E { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Title (W)")]
        public string Title_W { get; set; }

        [StringLength(2000)]
        [Required]
        [Display(Name = "Details (E)")]
        public string Content_E { get; set; }

        [StringLength(2000)]
        [Required]
        [Display(Name = "Details (W)")]
        public string Content_W { get; set; }

        public DateTime Published { get; set; }

        public List<PostImageViewModel> PostImages { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Venue (E)")]
        public string Venue_E { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Venue (W)")]
        public string Venue_W { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Address (E)")]
        public string Address_E { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Address (W)")]
        public string Address_W { get; set; }
    }
    
}
