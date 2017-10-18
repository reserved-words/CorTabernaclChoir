using System.Linq;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CorTabernaclChoir.Tests.Mappers
{
    [TestClass]
    public class EnglishMapperTest
    {
        [TestMethod]
        public void MapHome_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Home();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Home, HomeViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.MainText_E, result.MainText);
            Assert.AreEqual(testData.Accompanist, result.Accompanist);
            Assert.AreEqual(testData.MusicalDirector, result.MusicalDirector);
        }

        [TestMethod]
        public void MapJoin_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Join();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Join,JoinViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.MainText_E, result.MainText);
            Assert.AreEqual(testData.RehearsalTimes_E, result.RehearsalTimes);
            Assert.AreEqual(testData.Concerts_E, result.Concerts);
            Assert.AreEqual(testData.Subscriptions_E, result.Subscriptions);
            Assert.AreEqual(testData.Location_E, result.Location);
            Assert.AreEqual(testData.Auditions_E, result.Auditions);
        }

        [TestMethod]
        public void MapPost_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Posts().First();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Post, PostViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Type, result.Type);
            Assert.AreEqual(testData.Title_E, result.Title);
            Assert.AreEqual(testData.Content_E, result.Content);
            Assert.AreEqual(testData.Published, result.Published);
            Assert.AreEqual(testData.PostImages.First(), result.Images.First());
            Assert.AreEqual(testData.PostImages.Last(), result.Images.Last());
        }

        [TestMethod]
        public void MapEvent_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Events().First();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Event, EventViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Title_E, result.Title);
            Assert.AreEqual(testData.Date, result.Date);
            Assert.AreEqual(testData.Content_E, result.Details);
            Assert.AreEqual(testData.Venue_E, result.Venue);
            Assert.AreEqual(testData.Address_E, result.Address);
            Assert.AreEqual(testData.PostImages.First(), result.Images.First());
            Assert.AreEqual(testData.PostImages.Last(), result.Images.Last());
        }

        [TestMethod]
        public void MapSocialMedia_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.SocialMediaAccounts().First();
            
            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<SocialMediaAccount, SocialMediaViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Name, result.Name);
            Assert.AreEqual(testData.Url, result.Url);
            Assert.AreEqual(testData.ImageFileId, result.ImageFileId);
        }
    }
}
