using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.WorksTitle), nameof(Resources.MenuWorks))]
    public class WorksController : BaseController
    {
        private readonly ICultureService _cultureService;
        private readonly IWorksService _service;

        public WorksController(IWorksService service, ICultureService cultureService, ILogger logger, IMessageContainer messageContainer)
            : base(logger, messageContainer)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/Works/")]
        [WelshRoute("Gweithiau")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }

        [HttpGet]
        [Authorize]
        [Route("~/Works/Add/")]
        public ActionResult Add()
        {
            return View(new Work());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Works/Add/")]
        public ActionResult Add(Work model)
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
        [Route("~/Works/Edit/")]
        public ActionResult Edit(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Works/Edit/")]
        public ActionResult Edit(Work model)
        {
            if (ModelState.IsValid)
            {
                _service.Save(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        [Route("~/Works/Delete/")]
        public ActionResult Delete(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Works/Delete/")]
        public ActionResult Delete(Work model)
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