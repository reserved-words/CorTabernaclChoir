using System;
using System.Collections.Generic;
using System.Linq;
using CorTabernaclChoir.Common.Delegates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Services;
using CorTabernaclChoir.Data.Contracts;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class LayoutServiceTest
    {
        private readonly List<Post> _posts = TestData.Posts();
        private readonly List<SocialMediaAccount> _socialMediaAccounts = TestData.SocialMediaAccounts();
        private readonly Contact _contact = TestData.Contact();
        private readonly int _numberOfNewsItems = 5;
        private readonly int _numberOfEvents = 2;
        private readonly DateTime _currentDateTime = new DateTime(1999, 12, 25);

        private List<Event> _events;

        private LayoutService GetService(bool cultureIsWelsh)
        {
            _events = TestData.Events(_currentDateTime);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPostsRepository = new Mock<IRepository<Post>>();
            var mockEventsRepository = new Mock<IRepository<Event>>();
            var mockSocialMediaRepository = new Mock<IRepository<SocialMediaAccount>>();
            var mockContactRepository = new Mock<IRepository<Contact>>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockMapper = new Mock<IMapper>();

            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockPostsRepository.Object);
            mockUnitOfWork.Setup(u => u.Repository<Event>()).Returns(mockEventsRepository.Object);
            mockUnitOfWork.Setup(u => u.Repository<SocialMediaAccount>()).Returns(mockSocialMediaRepository.Object);
            mockUnitOfWork.Setup(u => u.Repository<Contact>()).Returns(mockContactRepository.Object);

            mockPostsRepository.Setup(r => r.GetAll()).Returns(_posts.AsQueryable());
            mockEventsRepository.Setup(r => r.GetAll()).Returns(_events.AsQueryable());
            mockSocialMediaRepository.Setup(r => r.GetAll()).Returns(_socialMediaAccounts.AsQueryable());
            mockContactRepository.Setup(r => r.GetSingle()).Returns(TestData.Contact());
            mockSystemVariablesService.Setup(s => s.NumberOfEventsInSidebar).Returns(_numberOfEvents);
            mockSystemVariablesService.Setup(s => s.NumberOfNewsItemsInSidebar).Returns(_numberOfNewsItems);
            mockSystemVariablesService.Setup(s => s.NumberOfNewsItemsInSidebar).Returns(_numberOfNewsItems);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(cultureIsWelsh);
            mockMapper.Setup(m => m.Map<Event, EventSummaryViewModel>(It.IsAny<Event>()))
                .Returns<Event>(e => new EventSummaryViewModel { Id = e.Id, Date = e.Date });
            mockMapper.Setup(m => m.Map<Post, PostSummaryViewModel>(It.IsAny<Post>()))
                .Returns<Post>(p => new PostSummaryViewModel {Id = p.Id, Published = p.Published });

            GetCurrentTime mockGetCurrentTime = () => _currentDateTime;

            return new LayoutService(
                () => mockUnitOfWork.Object,
                mockSystemVariablesService.Object,
                mockMapper.Object,
                mockGetCurrentTime,
                mockCultureService.Object);
        }

        [TestMethod]
        public void Get_GivenEnglishCulture_ReturnsCorrectModel()
        {
            // Arrange
            var sut = GetService(false);

            // Act
            var result = sut.GetSidebar();

            // Assert
            result.LatestNews.Count.Should().BeLessOrEqualTo(_numberOfNewsItems);
            result.UpcomingEvents.Count.Should().BeLessOrEqualTo(_numberOfEvents);
            result.SocialMediaLinks.Count.Should().Be(_socialMediaAccounts.Count);
            result.LatestNews.Should().BeInDescendingOrder(n => n.Published);
            result.UpcomingEvents.Should().BeInAscendingOrder(e => e.Date);
            result.UpcomingEvents.Max(e => e.Date).Should().BeAfter(_currentDateTime);
            result.ContactInformation.Should().Be(_contact.MainText_E);

            var firstNewsItem = result.LatestNews.First();
            var originalFirstNewsItem = _posts.Single(p => p.Id == firstNewsItem.Id);
            firstNewsItem.Id.Should().Be(originalFirstNewsItem.Id);

            var firstEvent = result.UpcomingEvents.First();
            var originalFirstEvent = _events.Single(p => p.Id == firstEvent.Id);
            firstEvent.Id.Should().Be(originalFirstEvent.Id);

            var firstSocialMedia = result.SocialMediaLinks.First();
            var originalFirstSocialMedia = _socialMediaAccounts.Single(p => p.Url == firstSocialMedia.Url);
            firstSocialMedia.Url.Should().Be(originalFirstSocialMedia.Url);
            firstSocialMedia.Name.Should().Be(originalFirstSocialMedia.Name);
        }

        [TestMethod]
        public void Get_GivenWelshCulture_ReturnsCorrectModel()
        {
            // Arrange
            var sut = GetService(true);

            // Act
            var result = sut.GetSidebar();

            // Assert
            result.LatestNews.Count.Should().BeLessOrEqualTo(_numberOfNewsItems);
            result.UpcomingEvents.Count.Should().BeLessOrEqualTo(_numberOfEvents);
            result.SocialMediaLinks.Count.Should().Be(_socialMediaAccounts.Count);
            result.LatestNews.Should().BeInDescendingOrder(n => n.Published);
            result.UpcomingEvents.Should().BeInAscendingOrder(e => e.Date);
            result.UpcomingEvents.Max(e => e.Date).Should().BeAfter(_currentDateTime);
            result.ContactInformation.Should().Be(_contact.MainText_W);

            var firstNewsItem = result.LatestNews.First();
            var originalFirstNewsItem = _posts.Single(p => p.Id == firstNewsItem.Id);
            firstNewsItem.Id.Should().Be(originalFirstNewsItem.Id);

            var firstEvent = result.UpcomingEvents.First();
            var originalFirstEvent = _events.Single(p => p.Id == firstEvent.Id);
            firstEvent.Id.Should().Be(originalFirstEvent.Id);

            var firstSocialMedia = result.SocialMediaLinks.First();
            var originalFirstSocialMedia = _socialMediaAccounts.Single(p => p.Url == firstSocialMedia.Url);
            firstSocialMedia.Url.Should().Be(originalFirstSocialMedia.Url);
            firstSocialMedia.Name.Should().Be(originalFirstSocialMedia.Name);
        }
    }
}
