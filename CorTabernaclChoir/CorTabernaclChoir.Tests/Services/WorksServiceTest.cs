using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class WorksServiceTest
    {
        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRepository<Work>>();
            var testData = TestData.Works();
            mockRepository.Setup(r => r.GetAll()).Returns(testData.AsQueryable());
            mockUnitOfWork.Setup(u => u.Repository<Work>()).Returns(mockRepository.Object);
            
            var sut = new WorksService(() => mockUnitOfWork.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(5, result.Years.Count);
            Assert.AreEqual(5, result.Years[0].Works.Count);
            Assert.AreEqual(5, result.Years[4].Works.Count);
            Assert.AreEqual(testData[0].Title, result.Years[0].Works[0].Title);
            Assert.AreEqual(testData[24].Composer, result.Years[4].Works[4].Composer);
        }
    }
}
