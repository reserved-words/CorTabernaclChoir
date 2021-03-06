﻿using System;
using System.Collections.Generic;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using CorTabernaclChoir.Common.Delegates;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class PostsServiceTest
    {
        private const int TestImageId = 5436;

        private readonly DateTime _mockCurrentTime = new DateTime(2017,1,1);
        private readonly List<Post> _testData = TestData.Posts();

        private readonly PostImage _testPostImage = new PostImage {Id = TestImageId};
        private readonly Post _testPostWithImage = new Post();

        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Post>> _mockPostsRepository;
        private Mock<IRepository<PostImage>> _mockImageRepository;
        private Mock<IAppSettingsService> _mockAppSettingsService;
        private Mock<IMapper> _mockMapper;

        private int _postsPerPage;

        private PostsService GetSubjectUnderTest(int postsPerPage = 5)
        {
            var mockCultureService = new Mock<ICultureService>();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<Post, PostViewModel>(It.IsAny<Post>()))
                .Returns<Post>(p => new PostViewModel { Id = p.Id });
            _mockMapper.Setup(m => m.Map<EditPostViewModel, Post>(It.IsAny<EditPostViewModel>()))
                .Returns<EditPostViewModel>(p => new Post { Id = p.Id });

            _postsPerPage = postsPerPage;
            _mockAppSettingsService = new Mock<IAppSettingsService>();
            _mockAppSettingsService.Setup(s => s.NumberOfItemsPerPage).Returns(_postsPerPage);

            _testPostWithImage.PostImages = new List<PostImage> { _testPostImage };
            _testPostImage.Post = _testPostWithImage;

            var mockGetCurrentTime = new GetCurrentTime(() => _mockCurrentTime);

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPostsRepository = new Mock<IRepository<Post>>();
            _mockImageRepository = new Mock<IRepository<PostImage>>();
            _mockPostsRepository.Setup(r => r.Including(n => n.PostImages)).Returns(_testData.AsQueryable());
            _mockImageRepository.Setup(r => r.GetById(TestImageId)).Returns(_testPostImage);
            _mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(_mockPostsRepository.Object);
            _mockUnitOfWork.Setup(u => u.Repository<PostImage>()).Returns(_mockImageRepository.Object);

            return new PostsService(() => _mockUnitOfWork.Object, mockCultureService.Object, _mockAppSettingsService.Object,
                _mockMapper.Object, mockGetCurrentTime);
        }

        [TestMethod]
        public void Get_GivenPage1News_ReturnsCorrectModel()
        {
            // Arrange
            var section = PostType.News;
            var pageNo = 1;
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Get(pageNo, section);

            // Assert
            Assert.AreEqual("News", result.ControllerName);
            Assert.AreEqual(pageNo, result.PageNo);
            Assert.IsNull(result.PreviousPage);
            Assert.AreEqual(pageNo + 1, result.NextPage);
            Assert.AreEqual(_postsPerPage, result.Items.Count);

            var itemsShouldBe = _testData
                .Where(n => n.Type == section)
                .OrderByDescending(t => t.Published)
                .Skip(_postsPerPage * (pageNo - 1))
                .Take(_postsPerPage)
                .ToList();

            Assert.AreEqual(itemsShouldBe.First().Id, result.Items.First().Id);
            Assert.AreEqual(itemsShouldBe.Last().Id, result.Items.Last().Id);
        }

        [TestMethod]
        public void Get_GivenLastPageVisits_ReturnsCorrectModel()
        {
            // Arrange
            var section = PostType.Visit;
            var pageNo = 3;
            var sut = GetSubjectUnderTest(10);

            // Act
            var result = sut.Get(pageNo, section);

            // Assert
            Assert.AreEqual(pageNo, result.PageNo);
            Assert.AreEqual(pageNo - 1, result.PreviousPage);
            Assert.IsNull(result.NextPage);
            Assert.AreEqual(5, result.Items.Count);
            Assert.AreEqual("Visits", result.ControllerName);

            var itemsShouldBe = _testData
                .Where(n => n.Type == section)
                .OrderByDescending(t => t.Published)
                .Skip(_postsPerPage * (pageNo - 1))
                .Take(_postsPerPage)
                .ToList();
            
            Assert.AreEqual(itemsShouldBe.First().Id, result.Items.First().Id);
            Assert.AreEqual(itemsShouldBe.Last().Id, result.Items.Last().Id);
        }

        [TestMethod]
        public void GetById_ReturnsCorrectModel()
        {
            // Arrange
            var testId = _testData[5].Id;

            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Get(testId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testId, result.Id);
        }

        [TestMethod]
        public void Save_GivenNewRecord_InsertsPost()
        {
            // Arrange
            var model = new EditPostViewModel { Id = 0 };
            var sut = GetSubjectUnderTest();

            // Act
            sut.Save(model);

            // Assert
            Assert.AreEqual(_mockCurrentTime, model.Published);
            _mockPostsRepository.Verify(r => r.Insert(It.Is<Post>(p => p.Id == 0)), Times.Once);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void Save_GivenExistingRecord_UpdatesPost()
        {
            // Arrange
            var id = _testData[10].Id;
            var deletedImageId = 55;
            var keepImageId = 56;
            var model = new EditPostViewModel
            {
                Id = id,
                PostImages = new List<PostImageViewModel>
                {
                    new PostImageViewModel
                    {
                        Id = keepImageId,
                        PostId = id,
                        MarkForDeletion = false
                    },
                    new PostImageViewModel
                    {
                        Id = deletedImageId,
                        PostId = id,
                        MarkForDeletion = true
                    }
                }
            };

            var sut = GetSubjectUnderTest();

            // Act
            sut.Save(model);

            // Assert
            _mockPostsRepository.Verify(r => r.Update(It.Is<Post>(p => p.Id == id)), Times.Once);
            _mockImageRepository.Verify(r => r.Delete(deletedImageId), Times.Once);
            _mockImageRepository.Verify(r => r.Delete(keepImageId), Times.Never);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void Delete_DeletesPost()
        {
            // Arrange
            var model = _testData[2];
            var sut = GetSubjectUnderTest();

            // Act
            sut.Delete(model);

            // Assert
            _mockPostsRepository.Verify(r => r.Delete(model), Times.Once);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void DeleteImage_DeletesImage()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            

            // Act
            sut.DeleteImage(TestImageId);

            // Assert
            _testPostWithImage.PostImages.Count(im => im.Id == TestImageId).Should().Be(0);
            _mockImageRepository.Verify(r => r.Delete(TestImageId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }
    }
}
