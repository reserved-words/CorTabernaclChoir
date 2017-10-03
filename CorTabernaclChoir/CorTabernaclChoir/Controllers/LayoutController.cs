using System.Web.Mvc;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Controllers
{
    public class LayoutController : Controller
    {
        private readonly ILayoutService _service;

        public LayoutController(ILayoutService service)
        {
            _service = service;
        }

        [Route("~/Layout/Sidebar")]
        [WelshRoute("Cynllun/BarOchr")]
        public ActionResult Sidebar(string culture)
        {
            return PartialView("_Sidebar", _service.GetSidebar());
        }

        [Route("~/Layout/Banner")]
        [WelshRoute("Cynllun/Baner")]
        public ActionResult Banner()
        {
            return PartialView("_Banner", _service.GetMusicalDirectorName());
        }
    }
}