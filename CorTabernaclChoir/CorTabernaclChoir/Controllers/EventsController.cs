using System;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.MenuNews), nameof(Resources.MenuNews))]
    public class EventsController : BaseController
    {
        private readonly ICultureService _cultureService;
        private readonly IPostsService _service;

        public EventsController(IPostsService service, ICultureService cultureService, ILogger logger)
            : base(logger)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/Events/{page}")]
        [WelshRoute("Digwyddiadau/{page}")]
        public ActionResult Index(string culture, int page)
        {
            throw new NotImplementedException();
        }

        [Route("~/Events/Item/{id}")]
        [WelshRoute("Digwyddiadau/Eitem/{id}")]
        public ActionResult Item(string culture, int id)
        {
            _cultureService.ValidateCulture(culture);

            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        [Route("~/Events/Add")]
        public ActionResult Add()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Post model)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        [Route("~/Events/Edit")]
        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post model)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("~/Events/Delete")]
        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Post model)
        {
            throw new NotImplementedException();
        }
    }
}