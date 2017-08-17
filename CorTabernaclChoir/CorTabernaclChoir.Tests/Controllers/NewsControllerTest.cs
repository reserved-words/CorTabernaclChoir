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
        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var pageNo = 1;
            var mockViewModel = new PostsViewModel { PageNo = pageNo, Items = new List<PostViewModel>() };
            var mockHandler = new Mock<IPostsService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockLogger = new Mock<ILogger>();
            var mockMessageContainer = new Mock<IMessageContainer>();
            var controller = new NewsController(mockHandler.Object, mockCultureService.Object, mockLogger.Object, mockMessageContainer.Object);
            mockHandler.Setup(h => h.Get(pageNo, PostType.News)).Returns(mockViewModel);

            // Act
            ViewResult result = controller.Index("en", 1) as ViewResult;

            // Assert
            mockHandler.Verify(h => h.Get(pageNo, PostType.News), Times.Once);
            mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(mockViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Index_GivenPageNo_ReturnsCorrectView()
        {
            // Arrange
            var pageNo = 2;
            var mockViewModel = new PostsViewModel { PageNo = pageNo, Items = new List<PostViewModel>() };
            var mockHandler = new Mock<IPostsService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockLogger = new Mock<ILogger>();
            var mockMessageContainer = new Mock<IMessageContainer>();
            var controller = new NewsController(mockHandler.Object, mockCultureService.Object, mockLogger.Object, mockMessageContainer.Object);
            mockHandler.Setup(h => h.Get(pageNo, PostType.News)).Returns(mockViewModel);

            // Act
            ViewResult result = controller.Index("en", pageNo) as ViewResult;

            // Assert
            mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);
            mockHandler.Verify(h => h.Get(pageNo, PostType.News), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(mockViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }
    }
}
