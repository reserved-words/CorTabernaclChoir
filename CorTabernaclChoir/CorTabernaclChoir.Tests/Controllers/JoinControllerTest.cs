using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Interfaces;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class JoinControllerTest
    {
        private const string TestErrorMessage = "Some error message";
        private const string TestPropertyName = "PropName";
        private const string RouteKeyAction = "Action";
        private const string ActionNameIndex = "Index";

        private Mock<IJoinService> _mockService;
        private Mock<ICultureService> _mockCultureService;
        private Mock<ILogger> _mockLogger;
        private Mock<IMessageContainer> _mockMessageContainer;

        private JoinViewModel _mockViewModel;
        private Join _mockModel;

        private JoinController GetSubjectUnderTest(bool modelIsValid = true)
        {
            _mockModel = new Join {MainText_E = "Main text in English", MainText_W = "Main text in Welsh"};
            _mockViewModel = new JoinViewModel { MainText = "ABC", RehearsalTimes = "sdkgjfh", Concerts = "slkdjg" };

            _mockService = new Mock<IJoinService>();
            _mockCultureService = new Mock<ICultureService>();
            _mockLogger = new Mock<ILogger>();
            _mockMessageContainer = new Mock<IMessageContainer>();

            _mockService.Setup(s => s.Get()).Returns(_mockViewModel);
            _mockService.Setup(s => s.GetForEdit()).Returns(_mockModel);

            var subjectUnderTest = new JoinController(_mockService.Object, _mockCultureService.Object, _mockLogger.Object, _mockMessageContainer.Object);

            if (!modelIsValid)
            {
                subjectUnderTest.ModelState.AddModelError(TestPropertyName, TestErrorMessage);
            }

            return subjectUnderTest;
        }

        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            
            // Act
            ViewResult result = sut.Index("en") as ViewResult;

            // Assert
            _mockService.Verify(h => h.Get(), Times.Once);
            _mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage(), Times.Never);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }
        
        [TestMethod]
        public void Edit_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Edit() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            var model = result.Model as Join;
            Assert.IsNotNull(model);
            Assert.AreEqual(_mockModel, model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void EditModel_GivenValidModel_CallsServiceAndRedirects()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Edit(_mockModel) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Save(_mockModel), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage(), Times.Once);
            result.Should().NotBeNull();
            result.RouteValues[RouteKeyAction].Should().Be(ActionNameIndex);
        }

        [TestMethod]
        public void EditModel_GivenInvalidModel_ReturnsViewWithErrorMessage()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Edit(_mockModel) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<Join>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            result.Model.Should().Be(_mockModel);
        }
    }
}
