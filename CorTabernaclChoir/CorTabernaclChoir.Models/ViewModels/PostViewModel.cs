using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorTabernaclChoir.Common.Models;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            Images = new List<int>();
        }

        public int Id { get; set; }
        public PostType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Published { get; set; }

        public List<int> Images { get; set; }
    }
}
