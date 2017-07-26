using System.Web.Mvc;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Controllers
{
    public class SidebarController : Controller
    {
        private readonly ISidebarService _service;

        public SidebarController(ISidebarService service)
        {
            _service = service;
        }

        [Route("~/Sidebar")]
        [WelshRoute("BarOchr")]
        public ActionResult Get(string culture)
        {
            return PartialView("_Sidebar", _service.Get());
        }
    }
}