using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class SidebarViewModel
    {
        public SidebarViewModel()
        {
            LatestNews = new List<PostSummaryViewModel>();
            UpcomingEvents = new List<EventSummaryViewModel>();
            SocialMediaLinks = new List<SocialMediaViewModel>();
        }

        public List<PostSummaryViewModel> LatestNews { get; set; }
        public List<EventSummaryViewModel> UpcomingEvents { get; set; }
        public List<SocialMediaViewModel> SocialMediaLinks { get; set; }
        public string ContactInformation { get; set; }
    }
}
