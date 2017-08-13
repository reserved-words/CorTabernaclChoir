using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;
using CorTabernaclChoir.Common;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class VisitsServiceTest
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
            var controller = new VisitsController(mockHandler.Object, mockCultureService.Object, mockLogger.Object);
            var section = PostType.Visit;

            mockHandler.Setup(h => h.Get(pageNo, section)).Returns(mockViewModel);

            // Act
            ViewResult result = controller.Index("en") as ViewResult;

            // Assert
            mockHandler.Verify(h => h.Get(pageNo, PostType.Visit), Times.Once);
            mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(mockViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }
    }
}
