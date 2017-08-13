using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CorTabernaclChoir.Common;

namespace CorTabernaclChoir.Logging
{
    public class Logger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(Exception ex, string message)
        {
            Console.WriteLine(ex.Message);
        }
    }
}