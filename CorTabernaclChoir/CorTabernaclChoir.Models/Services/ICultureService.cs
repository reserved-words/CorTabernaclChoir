using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Services
{
    public interface ICultureService
    {
        bool IsCurrentCultureWelsh();
        string ToggleCulture(string current = null);
        void ValidateCulture(string culture);
    }
}
