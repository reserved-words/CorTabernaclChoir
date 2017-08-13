using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.RecordingsTitle), nameof(Resources.MenuRecordings))]
    public class RecordingsController : BaseController
    {
        private readonly ICultureService _cultureService;
        private readonly IRecordingsService _service;

        public RecordingsController(IRecordingsService service, ICultureService cultureService, ILogger logger)
            : base(logger)
        {
            _cultureService = cultureService;
            _service = service;
        }

        [Route("~/Recordings/")]
        [WelshRoute("Recordiadau")]
        public ActionResult Index(string culture)
        {
            _cultureService.ValidateCulture(culture);

            return View(_service.Get());
        }
    }
}