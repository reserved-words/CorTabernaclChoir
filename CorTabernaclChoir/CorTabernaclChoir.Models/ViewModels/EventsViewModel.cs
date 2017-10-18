using CorTabernaclChoir.Common.Models;
using System;
using System.Collections.Generic;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class EventsViewModel
    {
        public int PageNo { get; set; }
        public List<EventViewModel> Items { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
    }
}