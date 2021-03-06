﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Models
{
    public class SocialMediaAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? ImageFileId { get; set; }

        public ImageFile ImageFile { get; set; }
    }
}
