using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class AboutControllerTest
    {
        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new AboutViewModel { AboutChoir = "ABC", AboutMusicalDirector = "Con", AboutAccompanist = "Acc" };
            var mockHandler = new Mock<IAboutService>();
            var mockCultureService = new Mock<ICultureService>();
            var controller = new AboutController(mockHandler.Object, mockCultureService.Object);
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
    }
}
