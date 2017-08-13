using System;
using System.Web.Mvc;
using CorTabernaclChoir.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorTabernaclChoir.Controllers;
using Moq;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new HomeViewModel { MainText = "ABC", MusicalDirector = "Con", Accompanist = "Acc" };
            var mockHandler = new Mock<IHomeService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockLogger = new Mock<ILogger>();
            var controller = new HomeController(mockHandler.Object, mockCultureService.Object, mockLogger.Object);
            mockHandler.Setup(h => h.Get()).Returns(mockViewModel);

            // Act
            ViewResult result = controller.Index("en") as ViewResult;

            // Assert
            mockHandler.Verify(h => h.Get(), Times.Once);
            mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(mockViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void ToggleLanguage_GivenEnglishCulture_CallsCultureService()
        {
            // Arrange
            var mockHandler = new Mock<IHomeService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockLogger = new Mock<ILogger>();
            var controller = new HomeController(mockHandler.Object, mockCultureService.Object, mockLogger.Object);

            // Act
            ViewResult result = controller.ToggleLanguage("en") as ViewResult;

            // Assert
            mockCultureService.Verify(c => c.ToggleCulture("en"), Times.Once);
        }

        [TestMethod]
        public void ToggleLanguage_GivenWelshCulture_CallsCultureService()
        {
            // Arrange
            var mockHandler = new Mock<IHomeService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockLogger = new Mock<ILogger>();
            var controller = new HomeController(mockHandler.Object, mockCultureService.Object, mockLogger.Object);

            // Act
            ViewResult result = controller.ToggleLanguage("cy") as ViewResult;

            // Assert
            mockCultureService.Verify(c => c.ToggleCulture("cy"), Times.Once);
        }
    }
}
