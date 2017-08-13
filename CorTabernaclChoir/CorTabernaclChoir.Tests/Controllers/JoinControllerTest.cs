using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using CorTabernaclChoir.Common;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class JoinControllerTest
    {
        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new JoinViewModel { MainText = "ABC", RehearsalTimes = "sdkgjfh", Concerts = "slkdjg" };

            var mockHandler = new Mock<IJoinService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockLogger = new Mock<ILogger>();
            var controller = new JoinController(mockHandler.Object, mockCultureService.Object, mockLogger.Object);
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
