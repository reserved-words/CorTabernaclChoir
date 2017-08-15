using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class GalleryServiceTest
    {
        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<IRepository<GalleryImage>>();
            var testData = TestData.GalleryImages();
            mockRepository.Setup(r => r.GetAll()).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<GalleryImage>()).Returns(mockRepository.Object);
            mockMapper.Setup(m => m.Map<GalleryImage, Image>(testData[0])).Returns(new Image { Id = testData[0].Id });

            var sut = new GalleryService(() => mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.Count, result.Images.Count);
            Assert.AreEqual(testData[0].Id, result.Images[0].Id);
        }
    }
}
