using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common
{
    public interface ILogger
    {
        void Info(string message);
        void Error(Exception ex, string requestUrl, string message);
    }
}
