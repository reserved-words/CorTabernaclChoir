using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
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
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<About>>();
            var mockMapper = new Mock<IMapper>();
            var testData = TestData.About();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<About>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);
            mockMapper.Setup(m => m.Map<About, AboutViewModel>(testData)).Returns(new AboutViewModel
            {
                AboutChoir = testData.AboutChoir_E,
                AboutMusicalDirector = testData.AboutMusicalDirector_E,
                AboutAccompanist = testData.AboutAccompanist_E
            });

            var sut = new AboutService(() => mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.AboutChoir_E, result.AboutChoir);
            Assert.AreEqual(testData.AboutMusicalDirector_E, result.AboutMusicalDirector);
            Assert.AreEqual(testData.AboutAccompanist_E, result.AboutAccompanist);
        }
    }
}
