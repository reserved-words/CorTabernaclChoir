using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorTabernaclChoir.Common.Services
{
    public interface IAppSettingsService
    {
        int NumberOfItemsPerPage { get; }
        string YouTubeApiKey { get; }
        string YouTubeChannelId { get; }
        int NumberOfVideosToDisplay { get; }
        int NumberOfNewsItemsInSidebar { get; }
        int NumberOfEventsInSidebar { get; }
    }
}
