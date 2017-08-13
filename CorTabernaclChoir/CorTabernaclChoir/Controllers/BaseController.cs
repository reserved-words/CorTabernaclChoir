using System.Web.Mvc;
using CorTabernaclChoir.Common;

namespace CorTabernaclChoir.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception, filterContext.HttpContext.Request.Url?.ToString(), "");

            filterContext.ExceptionHandled = true;

            filterContext.Result = Error();
        }

        [Route("~/Error")]
        public ActionResult Error()
        {
            return View("Error");
        }
    }
}