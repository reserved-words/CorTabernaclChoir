using System.Linq;
using System.Web.Mvc;
using CorTabernaclChoir.Controllers;
using CorTabernaclChoir.Messages;

namespace CorTabernaclChoir.Attributes
{
    public class MessageAttribute : ActionFilterAttribute
    {
        private const string MessageContainerKey = "Messages";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var  controller = filterContext.Controller as BaseController;

            if (controller != null)
            {
                controller.MessageContainer = controller.TempData[MessageContainerKey] as MessageContainer
                                    ?? new MessageContainer();
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;

            if (controller?.MessageContainer != null && controller.MessageContainer.Messages.Any())
            {
                if (filterContext.Result.GetType() == typeof(ViewResult))
                {
                    controller.ViewData[MessageContainerKey] = controller.MessageContainer;
                }
                else if (filterContext.Result.GetType() == typeof(RedirectToRouteResult))
                {
                    controller.TempData[MessageContainerKey] = controller.MessageContainer;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}