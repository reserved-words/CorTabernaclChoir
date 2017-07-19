using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System;
using System.Web.Mvc;

namespace CorTabernaclChoir.Controllers
{
    [ControllerInfo(nameof(Resources.ContactTitle), nameof(Resources.MenuContact))]
    public class ContactController : Controller
    {
        private readonly ICultureService _cultureService;
        private readonly IContactService _service;

        public ContactController(IContactService service, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/Contact/")]
        [WelshRoute("Cysylltu")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }

        [Route("~/Contact/SendForm")]
        public ActionResult SendForm(ContactFormViewModel model)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet]
        [Route("~/Contact/Edit")]
        public ActionResult Edit()
        {
            return View(_service.GetForEdit());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Contact/Edit")]
        public ActionResult Edit(Contact model)
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