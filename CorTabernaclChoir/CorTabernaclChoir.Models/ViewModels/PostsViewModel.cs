using CorTabernaclChoir.Common.Models;
using System;
using System.Collections.Generic;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class PostsViewModel
    {
        public int PageNo { get; set; }
        public List<PostViewModel> Items { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
        public string ControllerName { get; set; }
    }
}