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
    [Title(nameof(MenuNews), nameof(MenuNews))]
    public class NewsController : BaseController
    {
        private readonly ICultureService _cultureService;
        private readonly IPostsService _service;
        private readonly IUploadedFileValidator _uploadedFileValidator;
        private readonly IAppSettingsService _appSettings;
        private readonly IUploadedFileService _uploadedFileService;

        public NewsController(IPostsService service, ICultureService cultureService, ILogger logger, IMessageContainer messageContainer,
            IUploadedFileValidator uploadedFileValidator, IAppSettingsService appSettings, IUploadedFileService uploadedFileService)
            : base(logger, messageContainer)
        {
            _cultureService = cultureService;
            _service = service;
            _uploadedFileValidator = uploadedFileValidator;
            _appSettings = appSettings;
            _uploadedFileService = uploadedFileService;
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
            return View(new EditPostViewModel { Type = PostType.News });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/News/Add")]
        [Title(nameof(NewsAddTitle))]
        public ActionResult Add(EditPostViewModel model, HttpPostedFileBase image)
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
        [Route("~/News/Edit")]
        [Title(nameof(NewsEditTitle))]
        public ActionResult Edit(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/News/Edit")]
        [Title(nameof(NewsEditTitle))]
        public ActionResult Edit(EditPostViewModel model, HttpPostedFileBase image)
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
            return RedirectToAction(nameof(Index), new { culture = DefaultLanguage, page = 1 });
        }

        private void SavePost(EditPostViewModel model, HttpPostedFileBase image)
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