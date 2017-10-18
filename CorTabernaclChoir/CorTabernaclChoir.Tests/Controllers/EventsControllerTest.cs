using System;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Interfaces;
using FluentAssertions;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class EventsControllerTest
    {
        private const string TestErrorMessage = "Some error message";
        private const string TestPropertyName = "PropName";
        private const int TestPageNo = 4;
        private const int TestId = 23;
        private const int TestImageId = 504;
        private const string RouteKeyCulture = "culture";
        private const string RouteKeyPage = "page";
        private const string TestFileExtension = ".smt";

        private readonly EventsViewModel _mockEventsViewModel = new EventsViewModel { PageNo = TestPageNo, Items = new List<EventViewModel>() };
        private readonly EventViewModel _mockEventViewModel = new EventViewModel { Id = TestId };
        private readonly EditEventViewModel _mockEvent = new EditEventViewModel { Id = TestId};

        private Mock<IEventsService> _mockService;
        private Mock<ICultureService> _mockCultureService;
        private Mock<IMessageContainer> _mockMessageContainer;
        private Mock<IUploadedFileService> _mockUploadedFileService;
        private Mock<HttpPostedFileBase> _mockUploadedImage;

        private EventsController GetSubjectUnderTest(bool isModelValid = true, bool isImageUploaded = false, bool isUploadedImageValid = true)
        {
            _mockService = new Mock<IEventsService>();
            _mockCultureService = new Mock<ICultureService>();
            _mockMessageContainer = new Mock<IMessageContainer>();
            _mockUploadedFileService = new Mock<IUploadedFileService>();
            _mockUploadedImage = new Mock<HttpPostedFileBase>();

            var mockLogger = new Mock<ILogger>();
            var mockUploadedFileValidator = new Mock<IUploadedFileValidator>();
            var mockAppSettings = new Mock<IAppSettingsService>();

            var maxImageSize = 500;
            var validImageFileExtensions = new [] {".test"};

            _mockService.Setup(h => h.GetAll(TestPageNo)).Returns(_mockEventsViewModel);
            _mockService.Setup(h => h.GetById(TestId)).Returns(_mockEventViewModel);
            _mockService.Setup(h => h.GetForEdit(TestId)).Returns(_mockEvent);
            _mockService.Setup(s => s.Save(It.IsAny<EditEventViewModel>())).Returns(TestId);
            _mockService.Setup(s => s.SaveImage(TestId, It.IsAny<string>())).Returns(TestImageId);

            mockAppSettings.Setup(a => a.MaxPostImageFileSizeKB).Returns(maxImageSize);
            mockAppSettings.Setup(a => a.ValidPostImageFileExtensions).Returns(validImageFileExtensions);
            
            string error;

            mockUploadedFileValidator.Setup(v => v.IsFileUploaded(_mockUploadedImage.Object))
                .Returns(isImageUploaded);
            mockUploadedFileValidator.Setup(v => v.ValidateFile(_mockUploadedImage.Object, validImageFileExtensions, maxImageSize, out error))
                .Returns(isUploadedImageValid);
            mockUploadedFileValidator.Setup(v => v.GetFileExtension(_mockUploadedImage.Object)).Returns(TestFileExtension);

            var subjectUnderTest = new EventsController(_mockService.Object, _mockCultureService.Object, mockLogger.Object,
                _mockMessageContainer.Object, mockUploadedFileValidator.Object, mockAppSettings.Object, _mockUploadedFileService.Object);

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
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Index("en", TestPageNo) as ViewResult;

            // Assert
            _mockService.Verify(h => h.GetAll(TestPageNo), Times.Once);
            _mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockEventsViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Item_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Item("en", TestId) as ViewResult;

            // Assert
            _mockService.Verify(h => h.GetById(TestId), Times.Once);
            _mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockEventViewModel, result.Model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Add_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Add() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            var model = result.Model as EditEventViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Id);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void AddModel_GivenValidModelWithNoImage_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
            result.RouteValues[RouteKeyCulture].Should().Be(DefaultLanguage);
            result.RouteValues[RouteKeyPage].Should().Be(1);
        }

        [TestMethod]
        public void AddModel_GivenValidModelWithValidImage_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true);

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(TestId, TestFileExtension), Times.Once);
            _mockUploadedFileService.Verify(s => s.SaveImage(_mockUploadedImage.Object, ImageType.Post, TestImageId, TestFileExtension), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
            result.RouteValues[RouteKeyCulture].Should().Be(DefaultLanguage);
            result.RouteValues[RouteKeyPage].Should().Be(1);
        }

        [TestMethod]
        public void AddModel_GivenValidModelWithInvalidImage_ReturnsError()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true, false);

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditEventViewModel>()), Times.Never);
            _mockService.Verify(s => s.SaveImage(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            result.Model.Should().Be(model);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void AddModel_GivenInvalidModel_ReturnsError()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditEventViewModel>()), Times.Never);
            _mockService.Verify(s => s.SaveImage(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            result.Model.Should().Be(model);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void AddModel_GivenImageFileSaveFails_ReturnsError()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true);
            _mockUploadedFileService
                .Setup(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws<Exception>();

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(TestId, TestFileExtension), Times.Once);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), TestImageId, TestFileExtension), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            _mockService.Verify(s => s.DeleteImage(TestImageId), Times.Once);
            subjectUnderTest.ModelState.IsValid.Should().BeFalse();
            subjectUnderTest.ModelState[""].Errors.Count.Should().Be(1);
            subjectUnderTest.ModelState[""].Errors[0].ErrorMessage.Should().Be(PostImageSaveErrorMessage);
            result.Model.Should().Be(model);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void Edit_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Edit(TestId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            var model = result.Model as EditEventViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(_mockEvent, model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void EditModel_GivenValidModelWithNoImage_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
            result.RouteValues[RouteKeyCulture].Should().Be(DefaultLanguage);
            result.RouteValues[RouteKeyPage].Should().Be(1);
        }

        [TestMethod]
        public void EditModel_GivenValidModelWithValidImage_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true);

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(TestId, TestFileExtension), Times.Once);
            _mockUploadedFileService.Verify(s => s.SaveImage(_mockUploadedImage.Object, ImageType.Post, TestImageId, TestFileExtension), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
            result.RouteValues[RouteKeyCulture].Should().Be(DefaultLanguage);
            result.RouteValues[RouteKeyPage].Should().Be(1);
        }

        [TestMethod]
        public void EditModel_GivenModelWithInvalidImage_ReturnsError()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true, false);

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditEventViewModel>()), Times.Never);
            _mockService.Verify(s => s.SaveImage(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            result.Model.Should().Be(model);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void EditModel_GivenInvalidModel_ReturnsError()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditEventViewModel>()), Times.Never);
            _mockService.Verify(s => s.SaveImage(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            result.Model.Should().Be(model);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void EditModel_GivenImageFileSaveFails_ReturnsError()
        {
            // Arrange
            var model = new EditEventViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true);
            _mockUploadedFileService
                .Setup(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws<Exception>();

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(TestId, TestFileExtension), Times.Once);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), TestImageId, TestFileExtension), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            _mockService.Verify(s => s.DeleteImage(TestImageId), Times.Once);
            subjectUnderTest.ModelState.IsValid.Should().BeFalse();
            subjectUnderTest.ModelState[""].Errors.Count.Should().Be(1);
            subjectUnderTest.ModelState[""].Errors[0].ErrorMessage.Should().Be(PostImageSaveErrorMessage);
            result.Model.Should().Be(model);
            result.ViewName.Should().Be("");
        }

        [TestMethod]
        public void Delete_ReturnsCorrectView()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Delete(TestId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            var model = result.Model as EditEventViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(_mockEvent, model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void DeleteModel_GivenValidModel_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new Event();
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Delete(model) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Delete(model), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
            result.RouteValues[RouteKeyCulture].Should().Be(DefaultLanguage);
            result.RouteValues[RouteKeyPage].Should().Be(1);
        }
    }
}
