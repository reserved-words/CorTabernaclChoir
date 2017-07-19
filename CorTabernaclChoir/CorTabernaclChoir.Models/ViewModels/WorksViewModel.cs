using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.ViewModels
{
    public class WorksViewModel
    {
        public List<YearList> Years { get; set; }
    }

    public class YearList
    {
        public string Year { get; set; }
        public List<ChoralWork> Works { get; set; }
    }

    public class ChoralWork
    {
        public int Id { get; set; }
        public string Composer { get; set; }
        public string Title { get; set; }
    }
}