using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        private const string TestEmail = "test@email.com";
        private const string RouteKeyController = "Controller";
        private const string RouteKeyAction = "Action";

        [TestMethod]
        public void AddForwardingEmailAddress_CallsServiceAndRedirects()
        {
            // Arrange
            var mockService = new Mock<IEmailService>();
            var controller = new AdminController(mockService.Object);

            // Act
            var result = controller.AddForwardingEmailAddress(TestEmail);

            // Assert
            mockService.Verify(s => s.AddForwardingAddress(TestEmail), Times.Once);
            result.RouteValues[RouteKeyController].Should().BeNull();
            result.RouteValues[RouteKeyAction].Should().Be(nameof(controller.Index));
        }

        [TestMethod]
        public void RemoveForwardingEmailAddress_ReturnsCorrectView()
        {
            // Arrange
            var mockService = new Mock<IEmailService>();
            var controller = new AdminController(mockService.Object);

            // Act
            var result = controller.RemoveForwardingEmailAddress(TestEmail);

            // Assert
            mockService.Verify(s => s.RemoveForwardingAddress(TestEmail), Times.Never);
            result.Model.Should().Be(TestEmail);
            result.ViewName.Should().Be(string.Empty);
        }

        [TestMethod]
        public void ConfirmRemoveForwardingEmailAddress_CallsServiceAndRedirects()
        {
            // Arrange
            var mockService = new Mock<IEmailService>();
            var controller = new AdminController(mockService.Object);

            // Act
            var result = controller.ConfirmRemoveForwardingEmailAddress(TestEmail);

            // Assert
            mockService.Verify(s => s.RemoveForwardingAddress(TestEmail), Times.Once);
            result.RouteValues[RouteKeyController].Should().BeNull();
            result.RouteValues[RouteKeyAction].Should().Be(nameof(controller.Index));
        }
    }
}
