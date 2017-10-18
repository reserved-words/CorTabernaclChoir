using System;
using System.Web;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Interfaces;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(MenuEvents), nameof(MenuEvents))]
    public class EventsController : BaseController
    {
        private readonly ICultureService _cultureService;
        private readonly IEventsService _service;
        private readonly IUploadedFileValidator _uploadedFileValidator;
        private readonly IAppSettingsService _appSettings;
        private readonly IUploadedFileService _uploadedFileService;

        public EventsController(IEventsService service, ICultureService cultureService, ILogger logger, IMessageContainer messageContainer,
            IUploadedFileValidator uploadedFileValidator, IAppSettingsService appSettings, IUploadedFileService uploadedFileService)
            : base(logger, messageContainer)
        {
            _cultureService = cultureService;
            _service = service;
            _uploadedFileValidator = uploadedFileValidator;
            _appSettings = appSettings;
            _uploadedFileService = uploadedFileService;
        }

        [Route("~/Events/{page}")]
        [WelshRoute("Digwyddiadau/{page}")]
        public ActionResult Index(string culture, int page)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.GetAll(page));
        }

        [Route("~/Events/Item/{id}")]
        [WelshRoute("Digwyddiadau/Eitem/{id}")]
        public ActionResult Item(string culture, int id)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.GetById(id));
        }

        [HttpGet]
        [Authorize]
        [Route("~/Events/Add")]
        [Title(nameof(EventsAddTitle))]
        public ActionResult Add()
        {
            return View(new EditEventViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Events/Add")]
        [Title(nameof(EventsAddTitle))]
        public ActionResult Add(EditEventViewModel model, HttpPostedFileBase image)
        {
            ValidateUploadedImage(image);

            if (ModelState.IsValid)
            {
                SavePost(model, image);

                if (ModelState.IsValid)
                {
                    MessageContainer.AddSaveSuccessMessage();
                    return RedirectToIndex();
                }
            }

            MessageContainer.AddSaveErrorMessage();

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("~/Events/Edit")]
        [Title(nameof(EventsEditTitle))]
        public ActionResult Edit(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Events/Edit")]
        [Title(nameof(EventsEditTitle))]
        public ActionResult Edit(EditEventViewModel model, HttpPostedFileBase image)
        {
            ValidateUploadedImage(image);

            if (ModelState.IsValid)
            {
                SavePost(model, image);

                if (ModelState.IsValid)
                {
                    MessageContainer.AddSaveSuccessMessage();
                    return RedirectToIndex();
                }
            }

            MessageContainer.AddSaveErrorMessage();

            return View(model);
        }

        [Authorize]
        [Route("~/Events/Delete")]
        public ActionResult Delete(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Events/Delete")]
        public ActionResult Delete(Event model)
        {
            _service.Delete(model);

            MessageContainer.AddSaveSuccessMessage();

            return RedirectToIndex();
        }

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index), new { culture = DefaultLanguage, page = 1 });
        }

        private void SavePost(EditEventViewModel model, HttpPostedFileBase image)
        {
            var postId = _service.Save(model);

            if (!_uploadedFileValidator.IsFileUploaded(image))
                return;

            var imageFileExtension = _uploadedFileValidator.GetFileExtension(image);
            var imageId = _service.SaveImage(postId, imageFileExtension);

            try
            {
                _uploadedFileService.SaveImage(image, ImageType.Post, imageId, imageFileExtension);
            }
            catch (Exception ex)
            {
                // Log exception
                _service.DeleteImage(imageId);
                ModelState.AddModelError("", PostImageSaveErrorMessage);
            }
        }

        private void ValidateUploadedImage(HttpPostedFileBase file)
        {
            string errorMessage;

            if (_uploadedFileValidator.IsFileUploaded(file) && !_uploadedFileValidator.ValidateFile(
                file,
                _appSettings.ValidPostImageFileExtensions,
                _appSettings.MaxPostImageFileSizeKB,
                out errorMessage))
            {
                ModelState.AddModelError("", errorMessage);
            }
        }
    }
}