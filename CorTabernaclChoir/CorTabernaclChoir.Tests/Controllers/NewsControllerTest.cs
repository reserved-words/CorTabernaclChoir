using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class NewsServiceTest
    {
        private const int TestPageNo = 4;
        private const int TestId = 23;

        private readonly PostsViewModel _mockPostsViewModel = new PostsViewModel { PageNo = TestPageNo, Items = new List<PostViewModel>() };
        private readonly PostViewModel _mockPostViewModel = new PostViewModel {Id = 23};

        private Mock<IPostsService> _mockService;
        private Mock<ICultureService> _mockCultureService;

        private NewsController GetSubjectUnderTest()
        {
            _mockService = new Mock<IPostsService>();
            _mockCultureService = new Mock<ICultureService>();

            var mockLogger = new Mock<ILogger>();
            var mockMessageContainer = new Mock<IMessageContainer>();

            _mockService.Setup(h => h.Get(TestPageNo, PostType.News)).Returns(_mockPostsViewModel);
            _mockService.Setup(h => h.Get(TestId)).Returns(_mockPostViewModel);

            return new NewsController(_mockService.Object, _mockCultureService.Object, mockLogger.Object, mockMessageContainer.Object);
        }

        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Index("en", TestPageNo) as ViewResult;

            // Assert
            _mockService.Verify(h => h.Get(TestPageNo, PostType.News), Times.Once);
            _mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockPostsViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Item_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Item("en", TestId) as ViewResult;

            // Assert
            _mockService.Verify(h => h.Get(TestId), Times.Once);
            _mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockPostViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }
    }
}
