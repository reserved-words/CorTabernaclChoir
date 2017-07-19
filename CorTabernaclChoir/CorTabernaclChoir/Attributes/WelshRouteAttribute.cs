using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Attributes
{
    public class WelshRouteAttribute : Attribute
    {
        public WelshRouteAttribute(string route)
        {
            Route = route;
        }

        public string Route { get; }
    }
}