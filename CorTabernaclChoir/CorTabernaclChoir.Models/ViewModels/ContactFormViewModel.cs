using CorTabernaclChoir.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class ContactFormViewModel
    {
        [StringLength(100)]
        [Display(Name = nameof(Resources.ContactName), ResourceType = typeof(Resources))]
        public string Name { get; set; }

        [StringLength(255)]
        [EmailAddress(ErrorMessageResourceName = nameof(Resources.ContactInvalidEmailAddress), ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = nameof(Resources.ContactEmail), ResourceType = typeof(Resources))]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = nameof(Resources.ContactMessage), ResourceType = typeof(Resources))]
        public string Message { get; set; }
    }
}