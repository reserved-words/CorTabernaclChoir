using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Services;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class HomeServiceTest
    {
        [TestMethod]
        public void Get_GivenEnglishCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Home>>();
            var testData = TestData.Home();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Home>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new HomeService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.MainText_E, result.MainText);
            Assert.AreEqual(testData.MusicalDirector, result.Conductor);
            Assert.AreEqual(testData.Accompanist, result.Accompanist);
        }

        [TestMethod]
        public void Get_GivenWelshCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Home>>();
            var testData = TestData.Home();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Home>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new HomeService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.MainText_W, result.MainText);
            Assert.AreEqual(testData.MusicalDirector, result.Conductor);
            Assert.AreEqual(testData.Accompanist, result.Accompanist);
        }
    }
}
