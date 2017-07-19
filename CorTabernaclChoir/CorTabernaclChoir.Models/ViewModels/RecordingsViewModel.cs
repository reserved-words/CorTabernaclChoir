using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class RecordingsViewModel
    {
        public List<Video> Videos { get; set; }
    }

    public class Video
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
    }
}