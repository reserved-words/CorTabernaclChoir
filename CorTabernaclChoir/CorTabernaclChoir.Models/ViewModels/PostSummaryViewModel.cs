using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class PostSummaryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
    }
}
