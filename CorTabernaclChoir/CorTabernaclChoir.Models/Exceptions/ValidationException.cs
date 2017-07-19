using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Exceptions
{
    public class ValidationException : Exception
    {
        private const string ValidationUnexpectedError = "There was an unexpected error while saving the new image file";
        
        public ValidationException(string message)
            :base(message)
        {
        }

        public ValidationException()
            :this(ValidationUnexpectedError)
        {

        }
    }
}