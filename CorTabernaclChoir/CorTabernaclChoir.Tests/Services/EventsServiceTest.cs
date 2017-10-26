using System;
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
    public class EventsServiceTest
    {
        private const int TestImageId = 5436;

        private readonly DateTime _mockCurrentTime = new DateTime(2017, 1, 1);
        private readonly PostImage _testPostImage = new PostImage { Id = TestImageId };
        private readonly Event _testPostWithImage = new Event();

        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Event>> _mockEventsRepository;
        private Mock<IRepository<PostImage>> _mockImageRepository;
        private Mock<IMapper> _mockMapper;

        private List<Event> _testData;

        private EventsService GetSubjectUnderTest()
        {
            _testData = TestData.Events(_mockCurrentTime);

            var mockCultureService = new Mock<ICultureService>();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<Event, EventViewModel>(It.IsAny<Event>()))
                .Returns<Event>(p => new EventViewModel { Id = p.Id });
            _mockMapper.Setup(m => m.Map<Event, EventSummaryViewModel>(It.IsAny<Event>()))
                .Returns<Event>(p => new EventSummaryViewModel { Id = p.Id });
            _mockMapper.Setup(m => m.Map<Event, EditEventViewModel>(It.IsAny<Event>()))
                .Returns<Event>(p => new EditEventViewModel { Id = p.Id });
            _mockMapper.Setup(m => m.Map<EditEventViewModel, Event>(It.IsAny<EditEventViewModel>()))
                .Returns<EditEventViewModel>(p => new Event { Id = p.Id });

            _testPostWithImage.PostImages = new List<PostImage> { _testPostImage };
            _testPostImage.Post = _testPostWithImage;

            var mockGetCurrentTime = new GetCurrentTime(() => _mockCurrentTime);

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEventsRepository = new Mock<IRepository<Event>>();
            _mockImageRepository = new Mock<IRepository<PostImage>>();
            _mockEventsRepository.Setup(r => r.Including(n => n.PostImages)).Returns(_testData.AsQueryable());
            _mockImageRepository.Setup(r => r.GetById(TestImageId)).Returns(_testPostImage);
            _mockUnitOfWork.Setup(u => u.Repository<Event>()).Returns(_mockEventsRepository.Object);
            _mockUnitOfWork.Setup(u => u.Repository<PostImage>()).Returns(_mockImageRepository.Object);

            return new EventsService(() => _mockUnitOfWork.Object, _mockMapper.Object, mockGetCurrentTime);
        }

        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.GetAll();

            // Assert
            var upcomingItemsShouldBe = _testData
                .Where(t => t.Date >= _mockCurrentTime)
                .OrderBy(t => t.Date)
                .ToList();

            var pastItemsShouldBe = _testData
                .Where(t => t.Date < _mockCurrentTime)
                .OrderByDescending(t => t.Date)
                .ToList();

            Assert.AreEqual(upcomingItemsShouldBe.Count, result.Upcoming.Count);
            Assert.AreEqual(upcomingItemsShouldBe.First().Id, result.Upcoming.First().Id);
            Assert.AreEqual(upcomingItemsShouldBe.Last().Id, result.Upcoming.Last().Id);
            Assert.AreEqual(pastItemsShouldBe.Count, result.Past.Count);
            Assert.AreEqual(pastItemsShouldBe.First().Id, result.Past.First().Id);
            Assert.AreEqual(pastItemsShouldBe.Last().Id, result.Past.Last().Id);
        }

        [TestMethod]
        public void GetById_ReturnsCorrectModel()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var testId = _testData[5].Id;

            // Act
            var result = sut.GetById(testId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testId, result.Id);
        }

        [TestMethod]
        public void GetForEdit_ReturnsCorrectModel()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var testId = _testData[5].Id;

            // Act
            var result = sut.GetForEdit(testId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testId, result.Id);
        }

        [TestMethod]
        public void Save_GivenNewRecord_InsertsEvent()
        {
            // Arrange
            var model = new EditEventViewModel { Id = 0 };
            var sut = GetSubjectUnderTest();

            // Act
            sut.Save(model);

            // Assert
            _mockEventsRepository.Verify(r => r.Insert(It.Is<Event>(p => p.Id == 0 && p.Published == _mockCurrentTime)), Times.Once);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void Save_GivenExistingRecord_UpdatesPost()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var id = _testData[10].Id;
            var deletedImageId = 55;
            var keepImageId = 56;
            var model = new EditEventViewModel
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

            // Act
            sut.Save(model);

            // Assert
            _mockEventsRepository.Verify(r => r.Update(It.Is<Event>(p => p.Id == id)), Times.Once);
            _mockImageRepository.Verify(r => r.Delete(deletedImageId), Times.Once);
            _mockImageRepository.Verify(r => r.Delete(keepImageId), Times.Never);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void Delete_DeletesEvent()
        {
            // Arrange
            var sut = GetSubjectUnderTest();
            var model = _testData[2];

            // Act
            sut.Delete(model);

            // Assert
            _mockEventsRepository.Verify(r => r.Delete(model), Times.Once);
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
