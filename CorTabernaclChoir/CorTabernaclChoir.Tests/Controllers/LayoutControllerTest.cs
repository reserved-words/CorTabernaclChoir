using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorTabernaclChoir.Controllers;
using Moq;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class LayoutControllerTest
    {
        [TestMethod]
        public void Get_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new SidebarViewModel();
            var mockService = new Mock<ILayoutService>();
            var controller = new LayoutController(mockService.Object);
            mockService.Setup(h => h.GetSidebar()).Returns(mockViewModel);

            // Act
            var result = controller.Sidebar("") as PartialViewResult;

            // Assert
            mockService.Verify(h => h.GetSidebar(), Times.Once);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(mockViewModel, result.Model);
            Assert.AreEqual("_Sidebar", result.ViewName);
        }
    }
}
