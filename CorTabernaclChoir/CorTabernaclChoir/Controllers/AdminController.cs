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
    }
}