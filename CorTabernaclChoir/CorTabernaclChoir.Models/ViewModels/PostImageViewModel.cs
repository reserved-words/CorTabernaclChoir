using System.ComponentModel.DataAnnotations;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class PostImageViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string FileExtension { get; set; }
        [Display(Name = "Delete this image")]
        public bool MarkForDeletion { get; set; }
    }

}
