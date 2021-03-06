﻿using System.Web;
using System.Web.Mvc;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Interfaces;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Controllers
{
    [Authorize]
    public class SocialMediaController : BaseController
    {
        private const string ErrorMessageLogoRequired = "A logo is required";
        
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IUploadedFileValidator _uploadedFileValidator;
        private readonly ISocialMediaService _service;
        private readonly IAppSettingsService _appSettings;

        public SocialMediaController(ISocialMediaService service, IUploadedFileService uploadedFileService, 
            IUploadedFileValidator uploadedFileValidator, ILogger logger, IMessageContainer messageContainer,
            IAppSettingsService appSettings)
            : base(logger, messageContainer)
        {
            _service = service;
            _uploadedFileService = uploadedFileService;
            _uploadedFileValidator = uploadedFileValidator;
            _appSettings = appSettings;
        }

        [HttpGet]
        [Route("~/SocialMedia")]
        [Title(nameof(SocialMediaTitle))]
        public ActionResult Index()
        {
            return View(_service.GetAll());
        }

        [HttpGet]
        [Route("~/SocialMedia/Add")]
        [Title(nameof(SocialMediaAddTitle))]
        public ActionResult Add()
        {
            return View(new SocialMediaViewModel());
        }

        [HttpPost]
        [Route("~/SocialMedia/Add")]
        [ValidateAntiForgeryToken]
        [Title(nameof(SocialMediaAddTitle))]
        public ActionResult Add(SocialMediaViewModel model, HttpPostedFileBase image)
        {
            ValidateLogo(image, model.ImageFileId, nameof(model.ImageFileId));

            if (ModelState.IsValid)
            {
                var imageFile = image == null ? null : _uploadedFileService.Convert(image);

                _service.Add(model, imageFile);

                MessageContainer.AddSaveSuccessMessage();

                return RedirectToAction(nameof(Index));
            }

            MessageContainer.AddSaveErrorMessage();

            return View(model);
        }

        [Route("~/SocialMedia/Edit")]
        [Title(nameof(SocialMediaEditTitle))]
        public ActionResult Edit(int id)
        {
            var model = _service.Get(id);

            return View(model);
        }

        [HttpPost]
        [Route("~/SocialMedia/Edit")]
        [ValidateAntiForgeryToken]
        [Title(nameof(SocialMediaEditTitle))]
        public ActionResult Edit(SocialMediaViewModel model, HttpPostedFileBase logo)
        {
            ValidateLogo(logo, model.ImageFileId, nameof(model.ImageFileId));

            if (ModelState.IsValid)
            {
                var imageFile = logo == null ? null : _uploadedFileService.Convert(logo);

                _service.Edit(model, imageFile);

                MessageContainer.AddSaveSuccessMessage();

                return RedirectToAction(nameof(Index));
            }

            MessageContainer.AddSaveErrorMessage();

            return View(model);
        }

        [Route("~/SocialMedia/Delete")]
        [Title(nameof(SocialMediaDeleteTitle))]
        public ActionResult Delete(int id)
        {
            var model = _service.Get(id);

            return View(model);
        }

        [HttpPost]
        [Route("~/SocialMedia/Delete")]
        [ValidateAntiForgeryToken]
        [Title(nameof(SocialMediaDeleteTitle))]
        public ActionResult Delete(SocialMediaViewModel model)
        {
            _service.Delete(model.Id);

            MessageContainer.AddSaveSuccessMessage();

            return RedirectToAction(nameof(Index));
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
                if (!_uploadedFileValidator.ValidateSquareImage(logo, _appSettings.ValidLogoFileExtensions,
                    _appSettings.MinLogoWidth, _appSettings.MaxLogoWidth, _appSettings.MaxLogoFileSizeKB, out errorMessage))
                {
                    ModelState.AddModelError(propertyName, errorMessage);
                }
            }
        }
    }
}
