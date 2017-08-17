using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.AboutTitle),nameof(Resources.MenuAbout))]
    public class AboutController : BaseController
    {
        private readonly IAboutService _service;
        private readonly ICultureService _cultureService;

        public AboutController(IAboutService service, ICultureService cultureService, ILogger logger, IMessageContainer messageContainer)
            :base(logger, messageContainer)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/About/")]
        [WelshRoute("Amdanom")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }

        [Authorize]
        [HttpGet]
        [Route("~/About/Edit/")]
        public ActionResult Edit()
        {
            return View(_service.GetForEdit());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/About/Edit/")]
        public ActionResult Edit(About model)
        {
            if (ModelState.IsValid)
            {
                _service.Save(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}