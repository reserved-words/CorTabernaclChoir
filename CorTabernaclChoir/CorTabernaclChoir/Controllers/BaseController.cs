using System;
using System.Web.Mvc;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ILogger _logger;

        public IMessageContainer MessageContainer;

        protected BaseController(ILogger logger, IMessageContainer messageContainer)
        {
            _logger = logger;

            MessageContainer = messageContainer;
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

        protected void LogError(Exception ex, string message)
        {
            _logger.Error(ex, Request.Url?.ToString(), message);
        }
    }
}