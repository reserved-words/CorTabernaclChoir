using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class HomeViewModel
    {
        public string MainText { get; set; }

        [Display(Name = nameof(HomeMusicalDirector), ResourceType = typeof(Resources))]
        public string MusicalDirector { get; set; }

        [Display(Name = nameof(HomeAccompanist), ResourceType = typeof(Resources))]
        public string Accompanist { get; set; }
    }
}