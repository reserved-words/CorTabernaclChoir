using System.ComponentModel.DataAnnotations;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class SocialMediaViewModel
    {
        public int Id { get; set; }
        [Display(Name = "URL")]
        [Required]
        [Url]
        public string Url { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Name must not be more than {0} characters")]
        public string Name { get; set; }
        public int? ImageFileId { get; set; }

        public string Text => string.Format(Resources.SocialMediaTextFormat, Name);
    }
}
