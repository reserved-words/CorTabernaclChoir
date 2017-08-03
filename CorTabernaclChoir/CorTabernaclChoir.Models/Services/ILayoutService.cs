using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Common.Services
{
    public interface ILayoutService
    {
        SidebarViewModel GetSidebar();
        string GetMusicalDirectorName();
    }
}
