using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class EventViewModel
    {
        public EventViewModel()
        {
            Images = new List<int>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Venue { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public List<int> Images { get; set; }
    }
}
