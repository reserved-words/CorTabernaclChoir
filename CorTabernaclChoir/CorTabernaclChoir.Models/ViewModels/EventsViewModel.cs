using CorTabernaclChoir.Common.Models;
using System;
using System.Collections.Generic;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class EventsViewModel
    {
        public EventsViewModel()
        {
            Upcoming = new List<EventViewModel>();
            Past = new List<EventSummaryViewModel>();
        }

        public List<EventViewModel> Upcoming { get; set; }
        public List<EventSummaryViewModel> Past { get; set; }
    }
}