using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class EditPostViewModel
    {
        public EditPostViewModel()
        {
            PostImages = new List<PostImageViewModel>();
        }

        public int Id { get; set; }

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
        [Display(Name = "Content (E)")]
        public string Content_E { get; set; }

        [StringLength(2000)]
        [Required]
        [Display(Name = "Content (W)")]
        public string Content_W { get; set; }

        public DateTime Published { get; set; }

        public PostType Type { get; set; }

        public List<PostImageViewModel> PostImages { get; set; }
    }
    
}
