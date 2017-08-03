using System;
using System.Collections.Generic;
using System.Linq;
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
    public class LayoutServiceTest
    {
        private readonly List<Post> _posts = TestData.Posts();
        private readonly List<Event> _events = TestData.Events();
        private readonly List<SocialMediaAccount> _socialMediaAccounts = TestData.SocialMediaAccounts();
        private readonly Contact _contact = TestData.Contact();
        private readonly int _numberOfNewsItems = 5;
        private readonly int _numberOfEvents = 2;
        private readonly DateTime _currentDateTime = new DateTime(1999, 12, 25);

        private LayoutService GetService(bool cultureIsWelsh)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPostsRepository = new Mock<IRepository<Post>>();
            var mockEventsRepository = new Mock<IRepository<Event>>();
            var mockSocialMediaRepository = new Mock<IRepository<SocialMediaAccount>>();
            var mockContactRepository = new Mock<IRepository<Contact>>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockCultureService = new Mock<ICultureService>();

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
            GetCurrentTime mockGetCurrentTime = () => _currentDateTime;

            return new LayoutService(
                () => mockUnitOfWork.Object,
                mockSystemVariablesService.Object,
                mockCultureService.Object,
                mockGetCurrentTime);
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
            firstNewsItem.Title.Should().Be(originalFirstNewsItem.Title_E);

            var firstEvent = result.UpcomingEvents.First();
            var originalFirstEvent = _events.Single(p => p.Id == firstEvent.Id);
            firstEvent.Title.Should().Be(originalFirstEvent.Title_E);

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
            firstNewsItem.Title.Should().Be(originalFirstNewsItem.Title_W);

            var firstEvent = result.UpcomingEvents.First();
            var originalFirstEvent = _events.Single(p => p.Id == firstEvent.Id);
            firstEvent.Title.Should().Be(originalFirstEvent.Title_W);

            var firstSocialMedia = result.SocialMediaLinks.First();
            var originalFirstSocialMedia = _socialMediaAccounts.Single(p => p.Url == firstSocialMedia.Url);
            firstSocialMedia.Url.Should().Be(originalFirstSocialMedia.Url);
            firstSocialMedia.Name.Should().Be(originalFirstSocialMedia.Name);
        }
    }
}
