using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;

namespace CorTabernaclChoir.Controllers
{
    [ControllerInfo(nameof(Resources.VisitsTitle), nameof(Resources.MenuVisits))]
    public class VisitsController : Controller
    {
        private readonly ICultureService _cultureService;
        private readonly IPostsService _service;

        public VisitsController(IPostsService service, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/Visits/{page}")]
        [WelshRoute("Teithiau/{page}")]
        public ActionResult Index(string culture, int page = 1)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get(page, PostType.Visit));
        }

        [HttpGet]
        [Authorize]
        [Route("~/Visits/Add/")]
        public ActionResult Add()
        {
            return View(new Post { Type = PostType.Visit });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Visits/Add/")]
        public ActionResult Add(Post model)
        {
            if (ModelState.IsValid)
            {
                _service.Save(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("~/Visits/Edit/")]
        public ActionResult Edit(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Visits/Edit/")]
        public ActionResult Edit(Post model)
        {
            if (ModelState.IsValid)
            {
                _service.Save(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        [Route("~/Visits/Delete/")]
        public ActionResult Delete(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Visits/Delete/")]
        public ActionResult Delete(Post model)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}