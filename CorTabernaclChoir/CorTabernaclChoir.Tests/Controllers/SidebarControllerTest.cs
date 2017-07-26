using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorTabernaclChoir.Controllers;
using Moq;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class SidebarControllerTest
    {
        [TestMethod]
        public void Get_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new SidebarViewModel();
            var mockSidebarService = new Mock<ISidebarService>();
            var controller = new SidebarController(mockSidebarService.Object);
            mockSidebarService.Setup(h => h.Get()).Returns(mockViewModel);

            // Act
            var result = controller.Get("") as PartialViewResult;

            // Assert
            mockSidebarService.Verify(h => h.Get(), Times.Once);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(mockViewModel, result.Model);
            Assert.AreEqual("_Sidebar", result.ViewName);
        }
    }
}
