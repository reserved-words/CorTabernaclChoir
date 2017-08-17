using System.Web.Mvc;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImageService _service;

        public ImageController(IImageService service, ILogger logger, IMessageContainer messageContainer)
            : base(logger, messageContainer)
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