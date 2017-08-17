using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.JoinTitle), nameof(Resources.MenuJoin))]
    public class JoinController : BaseController
    {
        private readonly ICultureService _cultureService;
        private readonly IJoinService _service;

        public JoinController(IJoinService service, ICultureService cultureService, ILogger logger, IMessageContainer messageContainer)
            : base(logger, messageContainer)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/Join/")]
        [WelshRoute("Ymuno")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }

        [Authorize]
        [HttpGet]
        [Route("~/Join/Edit/")]
        public ActionResult Edit()
        {
            return View(_service.GetForEdit());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Join/Edit/")]
        public ActionResult Edit(Join model)
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