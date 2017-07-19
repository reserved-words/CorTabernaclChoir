using System;
using System.Collections.Generic;
using System.Linq;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Delegates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Services;
using CorTabernaclChoir.Data.Contracts;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class SidebarServiceTest
    {
        private readonly List<Post> _posts = TestData.Posts();
        private readonly List<Event> _events = TestData.Events();
        private readonly List<SocialMediaAccount> _socialMediaAccounts = TestData.SocialMediaAccounts();
        private readonly int _numberOfNewsItems = 5;
        private readonly int _numberOfEvents = 2;
        private readonly DateTime _currentDateTime = new DateTime(1999, 12, 25);

        [TestMethod]
        public void Get_GivenEnglishCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPostsRepository = new Mock<IRepository<Post>>();
            var mockEventsRepository = new Mock<IRepository<Event>>();
            var mockSocialMediaRepository = new Mock<IRepository<SocialMediaAccount>>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockCultureService = new Mock<ICultureService>();
            
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockPostsRepository.Object);
            mockUnitOfWork.Setup(u => u.Repository<Event>()).Returns(mockEventsRepository.Object);
            mockUnitOfWork.Setup(u => u.Repository<SocialMediaAccount>()).Returns(mockSocialMediaRepository.Object);

            mockPostsRepository.Setup(r => r.GetAll()).Returns(_posts.AsQueryable());
            mockEventsRepository.Setup(r => r.GetAll()).Returns(_events.AsQueryable());
            mockSocialMediaRepository.Setup(r => r.GetAll()).Returns(_socialMediaAccounts.AsQueryable());
            mockSystemVariablesService.Setup(s => s.NumberOfEventsInSidebar).Returns(_numberOfEvents);
            mockSystemVariablesService.Setup(s => s.NumberOfNewsItemsInSidebar).Returns(_numberOfNewsItems);
            mockSystemVariablesService.Setup(s => s.NumberOfNewsItemsInSidebar).Returns(_numberOfNewsItems);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);
            GetCurrentTime mockGetCurrentTime = () => _currentDateTime;

            var sut = new SidebarService(
                () => mockUnitOfWork.Object, 
                mockSystemVariablesService.Object,
                mockCultureService.Object,
                mockGetCurrentTime);

            // Act
            var result = sut.Get();

            // Assert
            result.LatestNews.Count.Should().BeLessOrEqualTo(_numberOfNewsItems);
            result.UpcomingEvents.Count.Should().BeLessOrEqualTo(_numberOfEvents);
            result.SocialMediaLinks.Count.Should().Be(_socialMediaAccounts.Count);
            result.LatestNews.Should().BeInDescendingOrder(n => n.Published);
            result.UpcomingEvents.Should().BeInAscendingOrder(e => e.Date);
            result.UpcomingEvents.Max(e => e.Date).Should().BeAfter(_currentDateTime);

            var firstNewsItem = result.LatestNews.First();
            var originalFirstNewsItem = _posts.Single(p => p.Id == firstNewsItem.Id);
            firstNewsItem.Title.Should().Be(originalFirstNewsItem.Title_E);

            var firstEvent = result.UpcomingEvents.First();
            var originalFirstEvent = _events.Single(p => p.Id == firstEvent.Id);
            firstEvent.Title.Should().Be(originalFirstEvent.Title_E);

            var firstSocialMedia = result.SocialMediaLinks.First();
            var originalFirstSocialMedia = _socialMediaAccounts.Single(p => p.Url == firstSocialMedia.Url);
            firstSocialMedia.ImageUrl.Should().Be(string.Format(Resources.SocialMediaImageUrl, originalFirstSocialMedia.Id));
        }

        [TestMethod]
        public void Get_GivenWelshCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPostsRepository = new Mock<IRepository<Post>>();
            var mockEventsRepository = new Mock<IRepository<Event>>();
            var mockSocialMediaRepository = new Mock<IRepository<SocialMediaAccount>>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockCultureService = new Mock<ICultureService>();

            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockPostsRepository.Object);
            mockUnitOfWork.Setup(u => u.Repository<Event>()).Returns(mockEventsRepository.Object);
            mockUnitOfWork.Setup(u => u.Repository<SocialMediaAccount>()).Returns(mockSocialMediaRepository.Object);

            mockPostsRepository.Setup(r => r.GetAll()).Returns(_posts.AsQueryable());
            mockEventsRepository.Setup(r => r.GetAll()).Returns(_events.AsQueryable());
            mockSocialMediaRepository.Setup(r => r.GetAll()).Returns(_socialMediaAccounts.AsQueryable());
            mockSystemVariablesService.Setup(s => s.NumberOfEventsInSidebar).Returns(_numberOfEvents);
            mockSystemVariablesService.Setup(s => s.NumberOfNewsItemsInSidebar).Returns(_numberOfNewsItems);
            mockSystemVariablesService.Setup(s => s.NumberOfNewsItemsInSidebar).Returns(_numberOfNewsItems);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);
            GetCurrentTime mockGetCurrentTime = () => _currentDateTime;

            var sut = new SidebarService(
                () => mockUnitOfWork.Object,
                mockSystemVariablesService.Object,
                mockCultureService.Object,
                mockGetCurrentTime);

            // Act
            var result = sut.Get();

            // Assert
            result.LatestNews.Count.Should().BeLessOrEqualTo(_numberOfNewsItems);
            result.UpcomingEvents.Count.Should().BeLessOrEqualTo(_numberOfEvents);
            result.SocialMediaLinks.Count.Should().Be(_socialMediaAccounts.Count);
            result.LatestNews.Should().BeInDescendingOrder(n => n.Published);
            result.UpcomingEvents.Should().BeInAscendingOrder(e => e.Date);
            result.UpcomingEvents.Max(e => e.Date).Should().BeAfter(_currentDateTime);

            var firstNewsItem = result.LatestNews.First();
            var originalFirstNewsItem = _posts.Single(p => p.Id == firstNewsItem.Id);
            firstNewsItem.Title.Should().Be(originalFirstNewsItem.Title_W);

            var firstEvent = result.UpcomingEvents.First();
            var originalFirstEvent = _events.Single(p => p.Id == firstEvent.Id);
            firstEvent.Title.Should().Be(originalFirstEvent.Title_W);

            var firstSocialMedia = result.SocialMediaLinks.First();
            var originalFirstSocialMedia = _socialMediaAccounts.Single(p => p.Url == firstSocialMedia.Url);
            firstSocialMedia.ImageUrl.Should().Be(string.Format(Resources.SocialMediaImageUrl, originalFirstSocialMedia.Id));
        }
    }
}
