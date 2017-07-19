using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;

namespace CorTabernaclChoir.Controllers
{
    [ControllerInfo(nameof(Resources.AdminTitle), nameof(Resources.AdminTitle))]
    [Authorize]
    public class AdminController : Controller
    {
        [Route("~/Admin/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}