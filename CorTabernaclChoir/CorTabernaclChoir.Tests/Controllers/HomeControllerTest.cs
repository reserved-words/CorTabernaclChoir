using System.Web.Mvc;
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
            var mockViewModel = new HomeViewModel { MainText = "ABC", Conductor = "Con", Accompanist = "Acc" };
            var mockHandler = new Mock<IHomeService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockSidebarService = new Mock<ISidebarService>();
            var controller = new HomeController(mockHandler.Object, mockCultureService.Object, mockSidebarService.Object);
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
            var mockSidebarService = new Mock<ISidebarService>();
            var controller = new HomeController(mockHandler.Object, mockCultureService.Object, mockSidebarService.Object);

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
            var mockSidebarService = new Mock<ISidebarService>();
            var controller = new HomeController(mockHandler.Object, mockCultureService.Object, mockSidebarService.Object);

            // Act
            ViewResult result = controller.ToggleLanguage("cy") as ViewResult;

            // Assert
            mockCultureService.Verify(c => c.ToggleCulture("cy"), Times.Once);
        }

        [TestMethod]
        public void Sidebar_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new SidebarViewModel();
            var mockService = new Mock<IHomeService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockSidebarService = new Mock<ISidebarService>();
            var controller = new HomeController(mockService.Object, mockCultureService.Object, mockSidebarService.Object);
            mockSidebarService.Setup(h => h.Get()).Returns(mockViewModel);

            // Act
            var result = controller.Sidebar("en") as PartialViewResult;

            // Assert
            mockSidebarService.Verify(h => h.Get(), Times.Once);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(mockViewModel, result.Model);
            Assert.AreEqual("_Sidebar", result.ViewName);
        }
    }
}
