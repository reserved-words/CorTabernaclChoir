using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Services;
using CorTabernaclChoir.Data.Contracts;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class HomeServiceTest
    {
        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<IRepository<Home>>();
            var testData = TestData.Home();
            var testModel = new HomeViewModel { };
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Home>()).Returns(mockRepository.Object);
            mockMapper.Setup(s => s.Map<Home,HomeViewModel>(testData)).Returns(testModel);

            var sut = new HomeService(() => mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testModel, result);
        }
    }
}
