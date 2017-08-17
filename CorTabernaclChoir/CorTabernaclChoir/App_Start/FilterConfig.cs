using System.Web.Mvc;
using CorTabernaclChoir.Attributes;

namespace CorTabernaclChoir
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MessageAttribute());
        }
    }
}
