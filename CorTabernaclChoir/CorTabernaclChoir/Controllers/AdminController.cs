using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.AdminTitle), nameof(Resources.AdminTitle))]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IEmailService _emailService;

        public AdminController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [Route("~/Admin/")]
        public ActionResult Index()
        {
            var model = new AdminViewModel
            {
                ForwardingEmailAddresses = _emailService.GetForwardingAddresses()
            };

            return View(model);
        }

        [HttpPost]
        [Route("~/Admin/AddForwardingEmailAddress")]
        public RedirectToRouteResult AddForwardingEmailAddress(string email)
        {
            _emailService.AddForwardingAddress(email);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("~/Admin/RemoveForwardingEmailAddress")]
        public ViewResult RemoveForwardingEmailAddress(string email)
        {
            return View(string.Empty, string.Empty, email);
        }

        [HttpPost]
        [Route("~/Admin/ConfirmRemoveForwardingEmailAddress")]
        public RedirectToRouteResult ConfirmRemoveForwardingEmailAddress(string email)
        {
            _emailService.RemoveForwardingAddress(email);

            return RedirectToAction(nameof(Index));
        }
    }
}