using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class ContactServiceTest
    {
        [TestMethod]
        public void Get_GivenEnglishCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Contact>>();
            var testData = TestData.Contact();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Contact>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new ContactService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.MainText_E, result.MainText);
        }

        [TestMethod]
        public void Get_GivenWelshCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Contact>>();
            var testData = TestData.Contact();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Contact>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new ContactService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.MainText_W, result.MainText);
        }
    }
}
