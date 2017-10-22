using System;
using System.Collections.Generic;
using System.Linq;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CorTabernaclChoir.Tests.Mappers
{
    [TestClass]
    public class PostMapperTest
    {
        [TestMethod]
        public void MapPostToEditPostViewModel_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Posts().First();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Post, EditPostViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Type, result.Type);
            Assert.AreEqual(testData.Title_E, result.Title_E);
            Assert.AreEqual(testData.Title_W, result.Title_W);
            Assert.AreEqual(testData.Content_E, result.Content_E);
            Assert.AreEqual(testData.Content_W, result.Content_W);
            Assert.AreEqual(testData.Published, result.Published);
            Assert.AreEqual(testData.PostImages.First().Id, result.PostImages.First().Id);
            Assert.AreEqual(testData.PostImages.Last().FileExtension, result.PostImages.Last().FileExtension);
            Assert.IsTrue(result.PostImages.All(im => !im.MarkForDeletion));
        }

        [TestMethod]
        public void MapEditPostViewModelToPost_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = new EditPostViewModel
            {
                Id = 345,
                Type = PostType.Event,
                Title_E = "Test Title",
                Content_W = "Cynnwys Cymraeg",
                Published = new DateTime(2014,1,5),
                PostImages = new List<PostImageViewModel>
                {
                    new PostImageViewModel
                    {
                        Id = 2,
                        FileExtension = ".jpg"
                    }
                }
            };
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<EditPostViewModel, Post>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Type, result.Type);
            Assert.AreEqual(testData.Title_E, result.Title_E);
            Assert.AreEqual(testData.Title_W, result.Title_W);
            Assert.AreEqual(testData.Content_E, result.Content_E);
            Assert.AreEqual(testData.Content_W, result.Content_W);
            Assert.AreEqual(testData.Published, result.Published);
            Assert.AreEqual(testData.PostImages.First().Id, result.PostImages.First().Id);
            Assert.AreEqual(testData.PostImages.Last().FileExtension, result.PostImages.Last().FileExtension);
        }

        [TestMethod]
        public void MapEventToEditEventViewModel_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = TestData.Events(new DateTime(2017,1,1)).First();
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<Event, EditEventViewModel>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Title_E, result.Title_E);
            Assert.AreEqual(testData.Title_W, result.Title_W);
            Assert.AreEqual(testData.Content_E, result.Content_E);
            Assert.AreEqual(testData.Content_W, result.Content_W);
            Assert.AreEqual(testData.Published, result.Published);
            Assert.AreEqual(testData.Date, result.Date);
            Assert.AreEqual(testData.Address_E, result.Address_E);
            Assert.AreEqual(testData.Address_W, result.Address_W);
            Assert.AreEqual(testData.Venue_E, result.Venue_E);
            Assert.AreEqual(testData.Venue_W, result.Venue_W);
            Assert.AreEqual(testData.PostImages.First().Id, result.PostImages.First().Id);
            Assert.AreEqual(testData.PostImages.Last().FileExtension, result.PostImages.Last().FileExtension);
            Assert.IsTrue(result.PostImages.All(im => !im.MarkForDeletion));
        }

        [TestMethod]
        public void MapEditEventViewModelToEvent_ReturnsCorrectModel()
        {
            // Arrange
            var mockCultureService = new Mock<ICultureService>();
            var testData = new EditEventViewModel
            {
                Id = 345,
                Title_E = "Test Title",
                Content_W = "Cynnwys Cymraeg",
                Published = new DateTime(2014, 1, 5),
                PostImages = new List<PostImageViewModel>
                {
                    new PostImageViewModel
                    {
                        Id = 2,
                        FileExtension = ".jpg"
                    }
                }
            };
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new Mapper.Mapper(mockCultureService.Object);

            // Act
            var result = sut.Map<EditEventViewModel, Event>(testData);

            // Assert
            Assert.AreEqual(testData.Id, result.Id);
            Assert.AreEqual(testData.Title_E, result.Title_E);
            Assert.AreEqual(testData.Title_W, result.Title_W);
            Assert.AreEqual(testData.Content_E, result.Content_E);
            Assert.AreEqual(testData.Content_W, result.Content_W);
            Assert.AreEqual(testData.Published, result.Published);
            Assert.AreEqual(testData.Address_E, result.Address_E);
            Assert.AreEqual(testData.Address_W, result.Address_W);
            Assert.AreEqual(testData.Venue_E, result.Venue_E);
            Assert.AreEqual(testData.Venue_W, result.Venue_W);
            Assert.AreEqual(testData.Published, result.Published);
            Assert.AreEqual(testData.PostImages.First().Id, result.PostImages.First().Id);
            Assert.AreEqual(testData.PostImages.Last().FileExtension, result.PostImages.Last().FileExtension);
        }
    }
}
