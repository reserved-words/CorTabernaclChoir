using System;
using CorTabernaclChoir.Common.Exceptions;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web;
using System.Web.Mvc;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.MenuGallery), nameof(Resources.MenuGallery))]
    public class GalleryController : Controller
    {
        private const string ImageGalleryFolder = "~/Images/Gallery";

        private readonly ICultureService _cultureService;
        private readonly IGalleryService _service;
        private readonly IImageFileService _imageFileService;

        private readonly string[] _validExtensions = { ".jpg", ".jpeg" };

        public GalleryController(IGalleryService service, ICultureService cultureService, IImageFileService imageFileService)
        {
            _cultureService = cultureService;
            _service = service;
            _imageFileService = imageFileService;
        }

        private string ImagesFolder => ControllerContext.HttpContext.Server.MapPath(ImageGalleryFolder);

        [Route("~/Gallery/")]
        [WelshRoute("Oriel")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }

        [HttpGet]
        [Authorize]
        [Route("~/Gallery/Add/")]
        public ActionResult Add()
        {
            return View(new GalleryImage());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Gallery/Add/")]
        public ActionResult Add(HttpPostedFileBase file, GalleryImage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tempFile = _imageFileService.Save(file, ImagesFolder, _validExtensions, model.Id == 0);

                    try
                    {
                        _service.Save(model);

                        if (string.IsNullOrEmpty(tempFile))
                        {
                            _imageFileService.Move(tempFile, ImagesFolder, model.Id);
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            _imageFileService.Delete(tempFile);
                        }
                        finally
                        {
                            throw new ValidationException();
                        }
                    }

                    _service.Save(model);

                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }    
            }

            return View(model);
        }
        
        [HttpGet]
        [Authorize]
        [Route("~/Gallery/Edit/")]
        public ActionResult Edit(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Gallery/Edit/")]
        public ActionResult Edit(HttpPostedFileBase file, GalleryImage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Save(model);

                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        [Authorize]
        [Route("~/Gallery/Delete/")]
        public ActionResult Delete(int id)
        {
            return View(_service.GetForEdit(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("~/Gallery/Delete/")]
        public ActionResult Delete(GalleryImage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Delete(model);

                    try
                    {
                        _imageFileService.Delete(ImagesFolder, model.Id);
                    }
                    catch (Exception)
                    {
                        throw new ValidationException();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }
    }
}