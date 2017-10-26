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
using CorTabernaclChoir.Interfaces;
using FluentAssertions;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class VisitsControllerTest
    {
        private const string TestErrorMessage = "Some error message";
        private const string TestPropertyName = "PropName";
        private const int TestPageNo = 4;
        private const int TestId = 23;
        private const int TestImageId = 504;
        private const string RouteKeyCulture = "culture";
        private const string RouteKeyPage = "page";
        private const string TestFileExtension = ".smt";
        
        private readonly PostsViewModel _mockPostsViewModel = new PostsViewModel { PageNo = TestPageNo, Items = new List<PostViewModel>() };
        private readonly PostViewModel _mockPostViewModel = new PostViewModel { Id = TestId };
        private readonly EditPostViewModel _mockPost = new EditPostViewModel { Id = TestId };
        private readonly Exception _imageSaveException = new Exception("Error saving image");
        private readonly Uri _testUri = new Uri("http://testUrl.com/");

        private Mock<IPostsService> _mockService;
        private Mock<ICultureService> _mockCultureService;
        private Mock<IMessageContainer> _mockMessageContainer;
        private Mock<IUploadedFileService> _mockUploadedFileService;
        private Mock<HttpPostedFileBase> _mockUploadedImage;
        private Mock<ILogger> _mockLogger;

        private VisitsController GetSubjectUnderTest(bool isModelValid = true, bool isImageUploaded = false, bool isUploadedImageValid = true)
        {
            _mockService = new Mock<IPostsService>();
            _mockCultureService = new Mock<ICultureService>();
            _mockMessageContainer = new Mock<IMessageContainer>();
            _mockUploadedFileService = new Mock<IUploadedFileService>();
            _mockUploadedImage = new Mock<HttpPostedFileBase>();
            _mockLogger = new Mock<ILogger>();

            var mockUploadedFileValidator = new Mock<IUploadedFileValidator>();
            var mockAppSettings = new Mock<IAppSettingsService>();

            var maxImageSize = 500;
            var validImageFileExtensions = new[] { ".test" };

            _mockService.Setup(h => h.Get(TestPageNo, PostType.Visit)).Returns(_mockPostsViewModel);
            _mockService.Setup(h => h.Get(TestId)).Returns(_mockPostViewModel);
            _mockService.Setup(h => h.GetForEdit(TestId)).Returns(_mockPost);
            _mockService.Setup(s => s.Save(It.IsAny<EditPostViewModel>())).Returns(TestId);
            _mockService.Setup(s => s.SaveImage(TestId, It.IsAny<string>())).Returns(TestImageId);

            mockAppSettings.Setup(a => a.MaxPostImageFileSizeKB).Returns(maxImageSize);
            mockAppSettings.Setup(a => a.ValidPostImageFileExtensions).Returns(validImageFileExtensions);

            string error;

            mockUploadedFileValidator.Setup(v => v.IsFileUploaded(_mockUploadedImage.Object))
                .Returns(isImageUploaded);
            mockUploadedFileValidator.Setup(v => v.ValidateFile(_mockUploadedImage.Object, validImageFileExtensions, maxImageSize, out error))
                .Returns(isUploadedImageValid);
            mockUploadedFileValidator.Setup(v => v.GetFileExtension(_mockUploadedImage.Object)).Returns(TestFileExtension);

            var subjectUnderTest = new VisitsController(_mockService.Object, _mockCultureService.Object, _mockLogger.Object,
                _mockMessageContainer.Object, mockUploadedFileValidator.Object, mockAppSettings.Object, _mockUploadedFileService.Object);

            if (!isModelValid)
            {
                subjectUnderTest.ModelState.AddModelError(TestPropertyName, TestErrorMessage);
            }

            subjectUnderTest.Setup(_testUri);

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
            _mockService.Verify(h => h.Get(TestPageNo, PostType.Visit), Times.Once);
            _mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockPostsViewModel, result.Model);
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
            _mockService.Verify(h => h.Get(TestId), Times.Once);
            _mockCultureService.Verify(c => c.ValidateCulture("en"), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockPostViewModel, result.Model);
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

            var model = result.Model as EditPostViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Id);
            Assert.AreEqual(model.Type, PostType.Visit);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void AddModel_GivenValidModelWithNoImage_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new EditPostViewModel();
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
            var model = new EditPostViewModel();
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
            var model = new EditPostViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true, false);

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditPostViewModel>()), Times.Never);
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
            var model = new EditPostViewModel();
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditPostViewModel>()), Times.Never);
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
            var model = new EditPostViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true);
            _mockUploadedFileService
                .Setup(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(_imageSaveException);

            // Act
            var result = subjectUnderTest.Add(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(TestId, TestFileExtension), Times.Once);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), TestImageId, TestFileExtension), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            _mockService.Verify(s => s.DeleteImage(TestImageId), Times.Once);
            _mockLogger.Verify(l => l.Error(_imageSaveException, _testUri.ToString(), It.IsAny<string>()), Times.Once);
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

            var model = result.Model as EditPostViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(_mockPost, model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void EditModel_GivenValidModelWithNoImage_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new EditPostViewModel();
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
            var model = new EditPostViewModel();
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
            var model = new EditPostViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true, false);

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditPostViewModel>()), Times.Never);
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
            var model = new EditPostViewModel();
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<EditPostViewModel>()), Times.Never);
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
            var model = new EditPostViewModel();
            var subjectUnderTest = GetSubjectUnderTest(true, true);
            _mockUploadedFileService
                .Setup(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(_imageSaveException);

            // Act
            var result = subjectUnderTest.Edit(model, _mockUploadedImage.Object) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockService.Verify(s => s.SaveImage(TestId, TestFileExtension), Times.Once);
            _mockUploadedFileService.Verify(s => s.SaveImage(It.IsAny<HttpPostedFileBase>(), It.IsAny<ImageType>(), TestImageId, TestFileExtension), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            _mockService.Verify(s => s.DeleteImage(TestImageId), Times.Once);
            _mockLogger.Verify(l => l.Error(_imageSaveException, _testUri.ToString(), It.IsAny<string>()), Times.Once);
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

            var model = result.Model as EditPostViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(_mockPost, model);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void DeleteModel_GivenValidModel_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new Post();
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
