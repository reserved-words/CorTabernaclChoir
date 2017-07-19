using CorTabernaclChoir.Common;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class GalleryServiceTest
    {
        [TestMethod]
        public void Get_GivenEnglishCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<GalleryImage>>();
            var testData = TestData.GalleryImages();
            mockRepository.Setup(r => r.GetAll()).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<GalleryImage>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new GalleryService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.Count, result.Images.Count);
            Assert.AreEqual(testData[0].Id, result.Images[0].Id);
            Assert.AreEqual(string.Format(Resources.GalleryImageCaption, testData[0].Caption_E, testData[0].Year), result.Images[0].Caption);
        }

        [TestMethod]
        public void Get_GivenWelshCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<GalleryImage>>();
            var testData = TestData.GalleryImages();
            mockRepository.Setup(r => r.GetAll()).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<GalleryImage>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new GalleryService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.Count, result.Images.Count);
            Assert.AreEqual(testData[0].Id, result.Images[0].Id);
            Assert.AreEqual(string.Format(Resources.GalleryImageCaption, testData[0].Caption_W, testData[0].Year), result.Images[0].Caption);
        }
    }
}
