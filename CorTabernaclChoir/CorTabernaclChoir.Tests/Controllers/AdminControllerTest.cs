﻿using CorTabernaclChoir.Common.Models;
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
        private const string RouteKeyController = "Controller";
        private const string RouteKeyAction = "Action";

        private readonly ContactEmail _testEmail = new ContactEmail { Id = 5, Address = "test@email.com" };

        [TestMethod]
        public void AddForwardingEmailAddress_CallsServiceAndRedirects()
        {
            // Arrange
            var mockService = new Mock<IEmailService>();
            var controller = new AdminController(mockService.Object);

            // Act
            var result = controller.AddEmailAddress(_testEmail);

            // Assert
            mockService.Verify(s => s.AddAddress(_testEmail), Times.Once);
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
            var result = controller.RemoveEmailAddress(_testEmail);

            // Assert
            mockService.Verify(s => s.RemoveAddress(It.IsAny<int>()), Times.Never);
            result.Model.Should().Be(_testEmail);
            result.ViewName.Should().Be(string.Empty);
        }

        [TestMethod]
        public void ConfirmRemoveForwardingEmailAddress_CallsServiceAndRedirects()
        {
            // Arrange
            var mockService = new Mock<IEmailService>();
            var controller = new AdminController(mockService.Object);

            // Act
            var result = controller.ConfirmRemoveEmailAddress(_testEmail.Id);

            // Assert
            mockService.Verify(s => s.RemoveAddress(_testEmail.Id), Times.Once);
            result.RouteValues[RouteKeyController].Should().BeNull();
            result.RouteValues[RouteKeyAction].Should().Be(nameof(controller.Index));
        }
    }
}
