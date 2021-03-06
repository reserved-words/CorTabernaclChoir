﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorTabernaclChoir.Common;
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
        private Mock<IMessageContainer> _mockMessageContainer;
        private string _errorMessage;

        private SocialMediaController GetSubjectUnderTest(bool isModelValid = true, bool isLogoValid = true)
        {
            _mockService = new Mock<ISocialMediaService>();
            _mockMessageContainer = new Mock<IMessageContainer>();

            var mockFileService = new Mock<IUploadedFileService>();
            var mockFileValidator = new Mock<IUploadedFileValidator>();
            var mockLogger = new Mock<ILogger>();
            var mockAppSettings = new Mock<IAppSettingsService>();

            var minLogoWidth = 5;
            var maxLogoWidth = 5000;
            var maxLogoSize = 500;
            var validLogoFileExtensions = new string[] {"", "k"};

            mockAppSettings.Setup(s => s.MinLogoWidth).Returns(minLogoWidth);
            mockAppSettings.Setup(s => s.MaxLogoWidth).Returns(maxLogoWidth);
            mockAppSettings.Setup(s => s.MaxLogoFileSizeKB).Returns(maxLogoSize);
            mockAppSettings.Setup(s => s.ValidLogoFileExtensions).Returns(validLogoFileExtensions);

            _mockService.Setup(s => s.GetAll()).Returns(new List<SocialMediaViewModel>
            {
                _testModel
            });
            _mockService.Setup(s => s.Get(TestId)).Returns(_testModel);

            mockFileService.Setup(s => s.Convert(_mockLogo.Object)).Returns(_testLogo);

            _errorMessage = TestErrorMessage;

            mockFileValidator.Setup(h => h.ValidateSquareImage(
                    It.IsAny<HttpPostedFileBase>(),
                    validLogoFileExtensions,
                    minLogoWidth,
                    maxLogoWidth,
                    maxLogoSize,
                    out _errorMessage))
                .Returns(isLogoValid);

            var subjectUnderTest = new SocialMediaController(_mockService.Object, mockFileService.Object, mockFileValidator.Object,
                mockLogger.Object, _mockMessageContainer.Object, mockAppSettings.Object);

            if (!isModelValid)
            {
                subjectUnderTest.ModelState.AddModelError(TestPropertyName, TestErrorMessage);
            }

            return subjectUnderTest;
        }

        [TestMethod]
        public void Index_ReturnsCorrectView()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Index() as ViewResult;

            // Assert
            var model = result.Model as List<SocialMediaViewModel>;
            model.Should().NotBeNull();
            model.Count.Should().Be(1);
            model.Single().ShouldBeEquivalentTo(_testModel);
            result.ViewName.Should().Be("");
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
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
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
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
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
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
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
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
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
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
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
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            result.Model.Should().Be(_testModel);
            result.ViewName.Should().Be("");
            subjectUnderTest.ViewData.ModelState[nameof(_testModel.ImageFileId)].Errors.Count.Should().Be(1);
            subjectUnderTest.ViewData.ModelState[nameof(_testModel.ImageFileId)].Errors[0].ErrorMessage.Should()
                .Be(TestErrorMessage);
        }

        [TestMethod]
        public void Delete_ReturnsCorrectView()
        {
            // Arrange
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Delete(TestId) as ViewResult;

            // Assert
            result.Model.Should().Be(_testModel);
            result.ViewName.Should().Be("");
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
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
        }
    }
}
