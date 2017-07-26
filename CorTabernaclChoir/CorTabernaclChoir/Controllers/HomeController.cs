using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using System.Web.Mvc;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Controllers
{
    [ControllerInfo(nameof(HomeTitle), nameof(MenuHome))]
    public class HomeController : Controller
    {
        private readonly ICultureService _cultureService;
        private readonly IHomeService _service;

        public HomeController(IHomeService service, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/")]
        [WelshRoute("Hafan")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }

        [Authorize]
        [HttpGet]
        [Route("~/Home/Edit")]
        public ActionResult Edit()
        {
            return View(_service.GetForEdit());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Home/Edit")]
        public ActionResult Edit(Home model)
        {
            if (ModelState.IsValid)
            {
                _service.Save(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        [Route("~/Home/ToggleLanguage")]
        [WelshRoute("Hafan/NewidIaith")]
        public ActionResult ToggleLanguage(string culture)
        {
            var newCulture = _cultureService.ToggleCulture(culture);

            return RedirectToAction(nameof(Index), new { culture = newCulture });
        }
    }
}