using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CorTabernaclChoir.Tests.Mappers
{
    [TestClass]
    public class WelshMapperTest
    {
        [TestMethod]
        public void MapHome_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Home();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Home, HomeViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.MainText_W, result.MainText);
            Assert.AreEqual(testData.Accompanist, result.Accompanist);
            Assert.AreEqual(testData.MusicalDirector, result.MusicalDirector);
        }

        [TestMethod]
        public void MapJoin_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Join();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Join, JoinViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.MainText_W, result.MainText);
            Assert.AreEqual(testData.RehearsalTimes_W, result.RehearsalTimes);
            Assert.AreEqual(testData.Concerts_W, result.Concerts);
            Assert.AreEqual(testData.Subscriptions_W, result.Subscriptions);
            Assert.AreEqual(testData.Location_W, result.Location);
            Assert.AreEqual(testData.Auditions_W, result.Auditions);
        }

        [TestMethod]
        public void MapPost_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Posts().First();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Post, PostViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Type, result.Type);
            Assert.AreEqual(testData.Title_W, result.Title);
            Assert.AreEqual(testData.Content_W, result.Content);
            Assert.AreEqual(testData.Published, result.Published);
            Assert.AreEqual(testData.PostImages.Select(im => im.Id).First(), result.Images.First());
            Assert.AreEqual(testData.PostImages.Select(im => im.Id).Last(), result.Images.Last());
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
