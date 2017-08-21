using System;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using CorTabernaclChoir.Common.Delegates;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class PostsServiceTest
    {
        private readonly DateTime _mockCurrentTime = new DateTime(2017,1,1);
        
        [TestMethod]
        public void Get_GivenPage1News_ReturnsCorrectModel()
        {
            // Arrange
            var section = PostType.News;
            var pageNo = 1;
            var postsPerPage = 5;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockMapper = new Mock<IMapper>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var mockGetCurrentTime = new GetCurrentTime(() => _mockCurrentTime);

            var testData = TestData.Posts();
            mockRepository.Setup(r => r.Including(n => n.PostImages)).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);
            mockSystemVariablesService.Setup(s => s.NumberOfItemsPerPage).Returns(postsPerPage);
            mockMapper.Setup(m => m.Map<Post, PostViewModel>(It.IsAny<Post>()))
                .Returns<Post>(p => new PostViewModel {Id = p.Id});

            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object, 
                mockMapper.Object, mockGetCurrentTime);
            
            // Act
            var result = sut.Get(pageNo, section);

            // Assert
            Assert.AreEqual("News", result.ControllerName);
            Assert.AreEqual(pageNo, result.PageNo);
            Assert.IsNull(result.PreviousPage);
            Assert.AreEqual(pageNo + 1, result.NextPage);
            Assert.AreEqual(postsPerPage, result.Items.Count);

            var itemsShouldBe = testData
                .Where(n => n.Type == section)
                .OrderByDescending(t => t.Published)
                .Skip(postsPerPage * (pageNo - 1))
                .Take(postsPerPage)
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
            var postsPerPage = 10;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockMapper = new Mock<IMapper>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var mockGetCurrentTime = new GetCurrentTime(() => _mockCurrentTime);

            var testData = TestData.Posts();
            mockRepository.Setup(r => r.Including(n => n.PostImages)).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);
            mockSystemVariablesService.Setup(s => s.NumberOfItemsPerPage).Returns(postsPerPage);
            mockMapper.Setup(m => m.Map<Post, PostViewModel>(It.IsAny<Post>()))
                .Returns<Post>(p => new PostViewModel { Id = p.Id });

            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object,
                mockMapper.Object, mockGetCurrentTime);

            // Act
            var result = sut.Get(pageNo, section);

            // Assert
            Assert.AreEqual(pageNo, result.PageNo);
            Assert.AreEqual(pageNo - 1, result.PreviousPage);
            Assert.IsNull(result.NextPage);
            Assert.AreEqual(5, result.Items.Count);
            Assert.AreEqual("Visits", result.ControllerName);

            var itemsShouldBe = testData
                .Where(n => n.Type == section)
                .OrderByDescending(t => t.Published)
                .Skip(postsPerPage * (pageNo - 1))
                .Take(postsPerPage)
                .ToList();
            
            Assert.AreEqual(itemsShouldBe.First().Id, result.Items.First().Id);
            Assert.AreEqual(itemsShouldBe.Last().Id, result.Items.Last().Id);
        }

        [TestMethod]
        public void GetById_ReturnsCorrectModel()
        {
            // Arrange
            var testData = TestData.Posts();
            var testId = testData[5].Id;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockMapper = new Mock<IMapper>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var mockGetCurrentTime = new GetCurrentTime(() => _mockCurrentTime);

            mockRepository.Setup(r => r.Including(n => n.PostImages)).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);
            mockMapper.Setup(m => m.Map<Post, PostViewModel>(It.IsAny<Post>()))
                .Returns<Post>(p => new PostViewModel { Id = p.Id });

            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object, 
                mockMapper.Object, mockGetCurrentTime);

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
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockMapper = new Mock<IMapper>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var mockGetCurrentTime = new GetCurrentTime(() => _mockCurrentTime);

            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);

            var model = new Post
            {
                Id = 0,
                Content_E = "New English Content",
                Content_W = "New Welsh Content"
            };

            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object, 
                mockMapper.Object, mockGetCurrentTime);

            // Act
            sut.Save(model);

            // Assert
            Assert.AreEqual(_mockCurrentTime, model.Published);
            mockRepository.Verify(r => r.Insert(model), Times.Once);
            mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }
    }
}
