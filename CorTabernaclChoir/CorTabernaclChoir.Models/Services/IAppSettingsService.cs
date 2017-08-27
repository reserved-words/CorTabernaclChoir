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
        string ValidGmailLogin { get; }
        int NumberOfVideosToDisplay { get; }
        int NumberOfNewsItemsInSidebar { get; }
        int NumberOfEventsInSidebar { get; }
        string[] ValidPostImageFileExtensions { get; }
        int MaxPostImageFileSizeKB { get; }
        int MinLogoWidth { get; }
        int MaxLogoWidth { get; }
        int MaxLogoFileSizeKB { get; }
        string[] ValidLogoFileExtensions { get; }
    }
}
