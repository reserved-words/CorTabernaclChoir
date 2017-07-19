using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Data.Contracts;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class JoinServiceTest
    {
        [TestMethod]
        public void Get_GivenEnglishCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Join>>();
            var testData = TestData.Join();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Join>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(false);

            var sut = new JoinService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.MainText_E, result.MainText);
            Assert.AreEqual(testData.RehearsalTimes_E, result.RehearsalTimes);
            Assert.AreEqual(testData.Concerts_E, result.Concerts);
            Assert.AreEqual(testData.Subscriptions_E, result.Subscriptions);
            Assert.AreEqual(testData.Location_E, result.Location);
            Assert.AreEqual(testData.Auditions_E, result.Auditions);
        }

        [TestMethod]
        public void Get_GivenWelshCulture_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockCultureService = new Mock<ICultureService>();
            var mockRepository = new Mock<IRepository<Join>>();
            var testData = TestData.Join();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Join>()).Returns(mockRepository.Object);
            mockCultureService.Setup(s => s.IsCurrentCultureWelsh()).Returns(true);

            var sut = new JoinService(() => mockUnitOfWork.Object, mockCultureService.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testData.MainText_W, result.MainText);
            Assert.AreEqual(testData.RehearsalTimes_W, result.RehearsalTimes);
            Assert.AreEqual(testData.Concerts_W, result.Concerts);
            Assert.AreEqual(testData.Subscriptions_W, result.Subscriptions);
            Assert.AreEqual(testData.Location_W, result.Location);
            Assert.AreEqual(testData.Auditions_W, result.Auditions);
        }
    }
}
