using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class AboutServiceTest
    {
        [TestMethod]
        public void Get_GivenEnglishCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<About>>();
            var testData = TestData.About();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<About>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new AboutService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.AboutChoir_E, result.AboutChoir);
            Assert.AreEqual(testData.AboutMusicalDirector_E, result.AboutConductor);
            Assert.AreEqual(testData.AboutAccompanist_E, result.AboutAccompanist);
        }

        [TestMethod]
        public void Get_GivenWelshCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<About>>();
            var testData = TestData.About();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<About>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new AboutService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.AboutChoir_W, result.AboutChoir);
            Assert.AreEqual(testData.AboutMusicalDirector_W, result.AboutConductor);
            Assert.AreEqual(testData.AboutAccompanist_W, result.AboutAccompanist);
        }
    }
}
