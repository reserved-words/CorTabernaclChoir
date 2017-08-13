﻿using System.Web.Mvc;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImageService _service;

        public ImageController(IImageService service, ILogger logger)
            : base(logger)
        {
            _service = service;
        }

        [Route("~/Image/Get")]
        public ActionResult Get(int id)
        {
            var image = _service.Get(id);

            return image == null
                ? null
                : File(image.File, image.ContentType);
        }
    }
}