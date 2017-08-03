using System;
using System.Linq;
using CorTabernaclChoir.Common.Delegates;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly ICultureService _cultureService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly GetCurrentTime _getCurrentTime;

        public LayoutService(Func<IUnitOfWork> unitOfWorkFactory, IAppSettingsService appSettingsService, 
            ICultureService cultureService, GetCurrentTime getCurrentTime)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _cultureService = cultureService;
            _appSettingsService = appSettingsService;
            _getCurrentTime = getCurrentTime;
        }

        public SidebarViewModel GetSidebar()
        {
            var welsh = _cultureService.IsCurrentCultureWelsh();

            using (var uow = _unitOfWorkFactory())
            {
                var socialMediaLinks = uow.Repository<SocialMediaAccount>().GetAll()
                    .ToList()
                    .Select(sm => new SocialMediaViewModel
                    {
                        Id = sm.Id,
                        Url = sm.Url,
                        Name = sm.Name,
                        ImageFileId = sm.ImageFileId
                    })
                    .ToList();

                var currentTime = _getCurrentTime();

                var upcomingEvents = uow.Repository<Event>().GetAll()
                    .Where(e => e.Date > currentTime)
                    .OrderBy(e => e.Date)
                    .Take(_appSettingsService.NumberOfEventsInSidebar)
                    .ToList()
                    .Select(e => new EventSummaryViewModel
                    {
                        Id = e.Id,
                        Title = welsh ? e.Title_W : e.Title_E,
                        Date = e.Date,
                        Venue = welsh ? e.Venue_W : e.Venue_E
                    })
                    .ToList();

                var latestNews = uow.Repository<Post>().GetAll()
                    .Where(p => p.Type == PostType.News)
                    .OrderByDescending(p => p.Published)
                    .Take(_appSettingsService.NumberOfNewsItemsInSidebar)
                    .ToList()
                    .Select(n => new PostSummaryViewModel
                    {
                        Id = n.Id,
                        Published = n.Published,
                        Title = welsh ? n.Title_W : n.Title_E
                    })
                    .ToList();

                var contact = uow.Repository<Contact>().GetSingle();

                return new SidebarViewModel
                {
                    SocialMediaLinks = socialMediaLinks,
                    UpcomingEvents = upcomingEvents,
                    LatestNews = latestNews,
                    ContactInformation = welsh ? contact.MainText_W : contact.MainText_E
                };
            }
        }

        public string GetMusicalDirectorName()
        {
            using (var uow = _unitOfWorkFactory())
            {
                return uow.Repository<Home>().GetSingle().MusicalDirector;
            }
        }
    }
}
