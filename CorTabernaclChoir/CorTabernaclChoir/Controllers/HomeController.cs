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
        private readonly ISidebarService _sidebarService;

        public HomeController(IHomeService service, ICultureService cultureService, ISidebarService sidebarService)
        {
            _cultureService = cultureService;
            _service = service;
            _sidebarService = sidebarService;
        }

        [Route("~/")]
        [WelshRoute("Hafan")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }

        [Route("~/Sidebar")]
        [WelshRoute("Hafan/BarOchr")]
        public ActionResult Sidebar(string culture)
        {
            return PartialView("_Sidebar", _sidebarService.Get());
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