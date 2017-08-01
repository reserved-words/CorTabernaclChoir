using System.Web.Mvc;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService _service;

        public ImageController(IImageService service)
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