using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;
using CorTabernaclChoir.Interfaces;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(MenuNews), nameof(MenuNews))]
    public class NewsController : BaseController
    {
        private readonly ICultureService _cultureService;
        private readonly IPostsService _service;

        public NewsController(IPostsService service, ICultureService cultureService, ILogger logger, IMessageContainer messageContainer)
            : base(logger, messageContainer)
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

            return View(_service.Get(id));
        }

        [HttpGet]
        [Authorize]
        [Route("~/News/Add")]
        [Title(nameof(NewsAddTitle))]
        public ActionResult Add()
        {
            return View(new Post { Type = PostType.News });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/News/Add")]
        [Title(nameof(NewsAddTitle))]
        public ActionResult Add(Post model)
        {
            if (ModelState.IsValid)
            {
                _service.Save(model);

                MessageContainer.AddSaveSuccessMessage();

                return RedirectToIndex();
            }

            MessageContainer.AddSaveErrorMessage();

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
        [Route("~/News/Edit")]
        public ActionResult Edit(Post model)
        {
            if (ModelState.IsValid)
            {
                _service.Save(model);

                MessageContainer.AddSaveSuccessMessage();

                return RedirectToIndex();
            }

            MessageContainer.AddSaveErrorMessage();

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
        [Route("~/News/Delete")]
        public ActionResult Delete(Post model)
        {
            _service.Delete(model);

            MessageContainer.AddSaveSuccessMessage();

            return RedirectToIndex();
        }

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index), new { culture = DefaultCulture, page = 1 });
        }
    }
}