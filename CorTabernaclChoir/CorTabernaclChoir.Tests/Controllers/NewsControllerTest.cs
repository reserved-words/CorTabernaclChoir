using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;
using CorTabernaclChoir.Common;
using CorTabernaclChoir.Interfaces;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Controllers
{
    [TestClass]
    public class NewsServiceTest
    {
        private const string TestErrorMessage = "Some error message";
        private const string TestPropertyName = "PropName";
        private const int TestPageNo = 4;
        private const int TestId = 23;

        private readonly PostsViewModel _mockPostsViewModel = new PostsViewModel { PageNo = TestPageNo, Items = new List<PostViewModel>() };
        private readonly PostViewModel _mockPostViewModel = new PostViewModel {Id = 23};

        private Mock<IPostsService> _mockService;
        private Mock<ICultureService> _mockCultureService;
        private Mock<IMessageContainer> _mockMessageContainer;

        private NewsController GetSubjectUnderTest(bool isModelValid = true)
        {
            _mockService = new Mock<IPostsService>();
            _mockCultureService = new Mock<ICultureService>();
            _mockMessageContainer = new Mock<IMessageContainer>();

            var mockLogger = new Mock<ILogger>();
            
            _mockService.Setup(h => h.Get(TestPageNo, PostType.News)).Returns(_mockPostsViewModel);
            _mockService.Setup(h => h.Get(TestId)).Returns(_mockPostViewModel);

            var subjectUnderTest = new NewsController(_mockService.Object, _mockCultureService.Object, mockLogger.Object, _mockMessageContainer.Object);

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
            _mockService.Verify(h => h.Get(TestPageNo, PostType.News), Times.Once);
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

            var model = result.Model as Post;
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Id);
            Assert.AreEqual(model.Type, PostType.News);
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void AddModel_GivenValidModel_CallsServiceAndRedirects()
        {
            // Arrange
            var model = new Post();
            var subjectUnderTest = GetSubjectUnderTest();

            // Act
            var result = subjectUnderTest.Add(model) as RedirectToRouteResult;

            // Assert
            _mockService.Verify(s => s.Save(model), Times.Once);
            _mockMessageContainer.Verify(m => m.AddSaveSuccessMessage());
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void AddModel_GivenInvalidModel_ReturnsError()
        {
            // Arrange
            var model = new Post();
            var subjectUnderTest = GetSubjectUnderTest(false);

            // Act
            var result = subjectUnderTest.Add(model) as ViewResult;

            // Assert
            _mockService.Verify(s => s.Save(It.IsAny<Post>()), Times.Never);
            _mockMessageContainer.Verify(m => m.AddSaveErrorMessage());
            result.Model.Should().Be(model);
            result.ViewName.Should().Be("");
        }
    }
}
