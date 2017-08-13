using System;

namespace CorTabernaclChoir.Common.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime LoggedAt { get; set; }
        public string RequestUrl { get; set; }
        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
