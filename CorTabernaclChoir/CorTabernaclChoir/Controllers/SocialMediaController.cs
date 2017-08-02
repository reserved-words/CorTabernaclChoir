using System;
using System.Web;
using System.Web.Mvc;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Interfaces;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Controllers
{
    [Authorize]
    public class SocialMediaController : Controller
    {
        private const string ErrorMessageLogoRequired = "A logo is required";
        private readonly IImageFileService _imageFileService;
        private readonly ISocialMediaService _service;

        public SocialMediaController(ISocialMediaService service, IImageFileService imageFileService)
        {
            _service = service;
            _imageFileService = imageFileService;
        }

        [HttpGet]
        [Route("~/SocialMedia/Add")]
        [Title(nameof(SocialMediaAddTitle), "")]
        public ActionResult Add()
        {
            return View(new SocialMediaViewModel());
        }

        [HttpPost]
        [Route("~/SocialMedia/Add")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SocialMediaViewModel model, HttpPostedFileBase logo)
        {
            ValidateLogo(logo, model.ImageFileId, nameof(model.ImageFileId));

            if (ModelState.IsValid)
            {
                var imageFile = logo == null ? null : _imageFileService.Convert(logo);

                _service.Add(model, imageFile);

                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }

        [Route("~/SocialMedia/Edit")]
        [Title(nameof(SocialMediaEditTitle), "")]
        public ActionResult Edit(int id)
        {
            var model = _service.Get(id);

            return View(model);
        }

        [HttpPost]
        [Route("~/SocialMedia/Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SocialMediaViewModel model, HttpPostedFileBase logo)
        {
            ValidateLogo(logo, model.ImageFileId, nameof(model.ImageFileId));

            if (ModelState.IsValid)
            {
                var imageFile = logo == null ? null : _imageFileService.Convert(logo);

                _service.Edit(model, imageFile);

                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SocialMediaViewModel model)
        {
            throw new NotImplementedException();
        }

        private void ValidateLogo(HttpPostedFileBase logo, int? existingId, string propertyName)
        {
            if (logo == null)
            {
                if (!existingId.HasValue)
                {
                    ModelState.AddModelError(propertyName, ErrorMessageLogoRequired);
                }
            }
            else
            {
                string errorMessage;
                if (!_imageFileService.ValidateFile(logo, out errorMessage))
                {
                    ModelState.AddModelError(propertyName, errorMessage);
                }
            }
        }
    }
}
