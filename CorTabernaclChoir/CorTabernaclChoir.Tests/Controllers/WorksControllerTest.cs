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
    public class WorksServiceTest
    {
        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var mockViewModel = new WorksViewModel
            {
                Years = new List<YearList>
                {
                    new YearList
                    {
                        Year = "2001",
                        Works = new List<ChoralWork>
                        {
                            new ChoralWork { Id = 1, Composer = "Hoohoo", Title = "Haha" }
                        }
                    }
                }
            };

            var mockHandler = new Mock<IWorksService>();
            var mockCultureService = new Mock<ICultureService>();
            var mockLogger = new Mock<ILogger>();
            var controller = new WorksController(mockHandler.Object, mockCultureService.Object, mockLogger.Object);
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
