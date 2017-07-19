using System.Linq;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Services
{
    public class RecordingsService : IRecordingsService
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly IYouTubeService _youTubeService;

        public RecordingsService(IAppSettingsService appSettingsService, IYouTubeService youTubeService)
        {
            _appSettingsService = appSettingsService;
            _youTubeService = youTubeService;
        }

        public RecordingsViewModel Get()
        {
            var videos = _youTubeService.GetVideos(
                _appSettingsService.YouTubeChannelId,
                _appSettingsService.YouTubeApiKey,
                _appSettingsService.NumberOfVideosToDisplay);

            return new RecordingsViewModel { Videos = videos.OrderByDescending(v => v.Published).ToList() };
        }
    }
}