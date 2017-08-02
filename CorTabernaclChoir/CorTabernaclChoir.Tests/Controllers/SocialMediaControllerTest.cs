using System.Web;
using System.Web.Mvc;
using CorTabernaclChoir.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorTabernaclChoir.Controllers;
using Moq;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Interfaces;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class SocialMediaControllerTest
    {
        private const string TestErrorMessage = "Some error message";
        private const string TestPropertyName = "PropName";
        private const int TestId = 153;

        private readonly SocialMediaViewModel _testModel = new SocialMediaViewModel { Name = "asfasf", Url = "http://www.google.com/" };
        private readonly ImageFile _testLogo = new ImageFile();
        private readonly Mock<HttpPostedFileBase> _mockLogo = new Mock<HttpPostedFileBase>();
        
        private Mock<ISocialMediaService> _mockService;
        private string _errorMessage;

        private SocialMediaController GetSubjectUnderTest(bool isModelValid = true, bool isLogoValid = true)
        {
            _mockService = new Mock<ISocialMediaService>();

            var mockImageFileService = new Mock<IImageFileService>();
            var subjectUnderTest = new SocialMediaController(_mockService.Object, mockImageFileService.Object);

            _mockService.Setup(s => s.Get(TestId)).Returns(_testModel);

            mockImageFileService.Setup(s => s.Convert(_mockLogo.Object)).Returns(_testLogo);

            _errorMessage = TestErrorMessage;

            if (isLogoValid)
            {
                mockImageFileService.Setup(h => h.ValidateFile(It.IsAny<HttpPostedFileBase>(), out _errorMessage))
                    .Returns(true);
            }
            else
            {
                mockImageFileService.Setup(h => h.ValidateFile(It.IsAny<HttpPostedFileBase>(), out _errorMessage))
                    .Returns(false);
            }

            if (!isModelValid)
            {
                subjectUnderTest.ModelState.AddModelError(TestPropertyName, TestErrorMessage);
            }

            return subjectUnderTest;
        }

        [TestMethod]
        public void Add_ReturnsCorrectView()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Add() as ViewResult;

            // Assert
            result.Model.ShouldBeEquivalentTo(new SocialMediaViewModel());
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void AddModel_InsertsAndRedirects()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Add(_testModel, _mockLogo.Object) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Add(_testModel, _testLogo), Times.Once);
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void AddModel_GivenInvalidModel_ReturnsError()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Add(_testModel, _mockLogo.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Add(It.IsAny<SocialMediaViewModel>(), It.IsAny<ImageFile>()), Times.Never);
            result.Model.Should().Be(_testModel);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void AddModel_GivenInvalidLogo_ReturnsError()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest(true, false);

            // Act
            var result = subjectUnderTest.Add(_testModel, _mockLogo.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Add(It.IsAny<SocialMediaViewModel>(), It.IsAny<ImageFile>()), Times.Never);
            result.Model.Should().Be(_testModel);
            result.ViewName.Should().Be("");
            subjectUnderTest.ViewData.ModelState[nameof(_testModel.ImageFileId)].Errors.Count.Should().Be(1);
            subjectUnderTest.ViewData.ModelState[nameof(_testModel.ImageFileId)].Errors[0].ErrorMessage.Should()
                .Be(TestErrorMessage);
        }

        [TestMethod]
        public void Edit_ReturnsCorrectView()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Edit(TestId) as ViewResult;

            // Assert
            result.Model.Should().Be(_testModel);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void EditModel_UpdatesAndRedirects()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Edit(_testModel, _mockLogo.Object) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Edit(_testModel, _testLogo), Times.Once);
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void EditModel_GivenInvalidModel_ReturnsError()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Edit(_testModel, _mockLogo.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Edit(It.IsAny<SocialMediaViewModel>(), It.IsAny<ImageFile>()), Times.Never);
            result.Model.Should().Be(_testModel);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void EditModel_GivenInvalidLogo_ReturnsError()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest(true, false);

            // Act
            var result = subjectUnderTest.Edit(_testModel, _mockLogo.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Edit(It.IsAny<SocialMediaViewModel>(), It.IsAny<ImageFile>()), Times.Never);
            result.Model.Should().Be(_testModel);
            result.ViewName.Should().Be("");
            subjectUnderTest.ViewData.ModelState[nameof(_testModel.ImageFileId)].Errors.Count.Should().Be(1);
            subjectUnderTest.ViewData.ModelState[nameof(_testModel.ImageFileId)].Errors[0].ErrorMessage.Should()
                .Be(TestErrorMessage);
        }

        [TestMethod]
        public void DeleteModel_DeletesAndRedirects()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Delete(_testModel) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Delete(_testModel.Id), Times.Once);
            result.Should().NotBeNull();
        }
    }
}
