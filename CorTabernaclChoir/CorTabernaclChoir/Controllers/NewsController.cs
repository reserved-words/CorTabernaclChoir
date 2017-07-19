using System;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;

namespace CorTabernaclChoir.Controllers
{
    [ControllerInfo(nameof(Resources.MenuNews), nameof(Resources.MenuNews))]
    public class NewsController : Controller
    {
        private readonly ICultureService _cultureService;
        private readonly IPostsService _service;

        public NewsController(IPostsService service, ICultureService cultureService)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/News/{page}")]
        [WelshRoute("Newyddion/{page}")]
        public ActionResult Index(string culture, int page)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get(page, PostType.News));
        }

        [Route("~/News/Item/{id}")]
        [WelshRoute("Newyddion/Eitem/{id}")]
        public ActionResult Item(string culture, int id)
        {
            _cultureService.ValidateCulture(culture);

            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        [Route("~/News/Add")]
        public ActionResult Add()
        {
            return View(new Post { Type = PostType.News });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
        [Route("~/News/Edit")]
        public ActionResult Edit(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
        [Route("~/News/Delete")]
        public ActionResult Delete(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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