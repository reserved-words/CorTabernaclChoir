using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Models
{
    public enum PostType { News, Visit, Event }

    public class Post
    {
        public Post()
        {
            PostImages = new List<PostImage>();
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

        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}