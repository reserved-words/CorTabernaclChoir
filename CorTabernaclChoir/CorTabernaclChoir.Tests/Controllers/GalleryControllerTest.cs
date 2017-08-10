using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorTabernaclChoir.Controllers;
using Moq;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class GalleryControllerTest
    {
        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new GalleryViewModel();
            var mockHandler = new Mock<IGalleryService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockImageSaveService = new Mock<IUploadedFileService>();
            var controller = new GalleryController(mockHandler.Object, mockCultureService.Object, mockImageSaveService.Object);
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
