using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class PostsServiceTest
    {
        [TestMethod]
        public void Get_GivenPage1EnglishNews_ReturnsCorrectModel()
        {
            // Arrange
            var section = PostType.News;
            var pageNo = 1;
            var postsPerPage = 5;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var testData = TestData.Posts();
            mockRepository.Setup(r => r.Including(n => n.PostImages)).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);
            mockSystemVariablesService.Setup(s => s.NumberOfItemsPerPage).Returns(postsPerPage);

            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object);
            
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

            Assert.AreEqual(itemsShouldBe.First().Title_E, result.Items.First().Title);
            Assert.AreEqual(itemsShouldBe.First().Content_E, result.Items.First().Content);
            Assert.AreEqual(itemsShouldBe.First().Published, result.Items.First().Published);
            Assert.AreEqual(itemsShouldBe.First().PostImages.Count, result.Items.First().Images.Count);
            Assert.AreEqual(itemsShouldBe.Last().Title_E, result.Items.Last().Title);
            Assert.AreEqual(itemsShouldBe.Last().Content_E, result.Items.Last().Content);
            Assert.AreEqual(itemsShouldBe.Last().Published, result.Items.Last().Published);
            Assert.AreEqual(itemsShouldBe.Last().PostImages.Count, result.Items.Last().Images.Count);
        }

        [TestMethod]
        public void Get_GivenLastPageEnglishVisits_ReturnsCorrectModel()
        {
            // Arrange
            var section = PostType.Visit;
            var pageNo = 3;
            var postsPerPage = 10;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var testData = TestData.Posts();
            mockRepository.Setup(r => r.Including(n => n.PostImages)).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);
            mockSystemVariablesService.Setup(s => s.NumberOfItemsPerPage).Returns(postsPerPage);

            
            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object);

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

            Assert.AreEqual(itemsShouldBe.First().Title_E, result.Items.First().Title);
            Assert.AreEqual(itemsShouldBe.First().Content_E, result.Items.First().Content);
            Assert.AreEqual(itemsShouldBe.First().Published, result.Items.First().Published);
            Assert.AreEqual(itemsShouldBe.First().PostImages.Count, result.Items.First().Images.Count);
            Assert.AreEqual(itemsShouldBe.Last().Title_E, result.Items.Last().Title);
            Assert.AreEqual(itemsShouldBe.Last().Content_E, result.Items.Last().Content);
            Assert.AreEqual(itemsShouldBe.Last().Published, result.Items.Last().Published);
            Assert.AreEqual(itemsShouldBe.Last().PostImages.Count, result.Items.Last().Images.Count);
        }

        [TestMethod]
        public void Get_GivenPage1WelshNews_ReturnsCorrectModel()
        {
            // Arrange
            var section = PostType.News;
            var pageNo = 1;
            var postsPerPage = 5;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var testData = TestData.Posts();
            mockRepository.Setup(r => r.Including(n => n.PostImages)).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);
            mockSystemVariablesService.Setup(s => s.NumberOfItemsPerPage).Returns(postsPerPage);

            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object);

            // Act
            var result = sut.Get(pageNo, section);

            // Assert
            Assert.AreEqual(pageNo, result.PageNo);
            Assert.IsNull(result.PreviousPage);
            Assert.AreEqual(pageNo + 1, result.NextPage);
            Assert.AreEqual(postsPerPage, result.Items.Count);
            Assert.AreEqual("Newyddion", result.ControllerName);

            var itemsShouldBe = testData
                .Where(n => n.Type == section)
                .OrderByDescending(t => t.Published)
                .Skip(postsPerPage * (pageNo - 1))
                .Take(postsPerPage)
                .ToList();

            Assert.AreEqual(itemsShouldBe.First().Title_W, result.Items.First().Title);
            Assert.AreEqual(itemsShouldBe.First().Content_W, result.Items.First().Content);
            Assert.AreEqual(itemsShouldBe.First().Published, result.Items.First().Published);
            Assert.AreEqual(itemsShouldBe.First().PostImages.Count, result.Items.First().Images.Count);
            Assert.AreEqual(itemsShouldBe.Last().Title_W, result.Items.Last().Title);
            Assert.AreEqual(itemsShouldBe.Last().Content_W, result.Items.Last().Content);
            Assert.AreEqual(itemsShouldBe.Last().Published, result.Items.Last().Published);
            Assert.AreEqual(itemsShouldBe.Last().PostImages.Count, result.Items.Last().Images.Count);
        }

        [TestMethod]
        public void Get_GivenLastPageWelshVisits_ReturnsCorrectModel()
        {
            // Arrange
            var section = PostType.Visit;
            var pageNo = 5;
            var postsPerPage = 5;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Post>>();
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var testData = TestData.Posts();
            mockRepository.Setup(r => r.Including(n => n.PostImages)).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Post>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);
            mockSystemVariablesService.Setup(s => s.NumberOfItemsPerPage).Returns(postsPerPage);

            var sut = new PostsService(() => mockUnitOfWork.Object, mockCultureService.Object, mockSystemVariablesService.Object);

            // Act
            var result = sut.Get(pageNo, section);

            // Assert
            Assert.AreEqual(pageNo, result.PageNo);
            Assert.AreEqual(pageNo - 1, result.PreviousPage);
            Assert.IsNull(result.NextPage);
            Assert.AreEqual(5, result.Items.Count);
            Assert.AreEqual("Teithiau", result.ControllerName);

            var itemsShouldBe = testData
                .Where(n => n.Type == section)
                .OrderByDescending(t => t.Published)
                .Skip(postsPerPage * (pageNo - 1))
                .Take(postsPerPage)
                .ToList();

            Assert.AreEqual(itemsShouldBe.First().Title_W, result.Items.First().Title);
            Assert.AreEqual(itemsShouldBe.First().Content_W, result.Items.First().Content);
            Assert.AreEqual(itemsShouldBe.First().Published, result.Items.First().Published);
            Assert.AreEqual(itemsShouldBe.First().PostImages.Count, result.Items.First().Images.Count);
            Assert.AreEqual(itemsShouldBe.Last().Title_W, result.Items.Last().Title);
            Assert.AreEqual(itemsShouldBe.Last().Content_W, result.Items.Last().Content);
            Assert.AreEqual(itemsShouldBe.Last().Published, result.Items.Last().Published);
            Assert.AreEqual(itemsShouldBe.Last().PostImages.Count, result.Items.Last().Images.Count);
        }
    }
}
