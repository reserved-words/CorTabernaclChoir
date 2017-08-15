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
    public class JoinServiceTest
    {
        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<IRepository<Join>>();
            var testData = TestData.Join();
            var testViewModel = new JoinViewModel();
            mockRepository.Setup(r => r.GetSingle()).Returns(testData);
            mockUnitOfWork.Setup(u => u.Repository<Join>()).Returns(mockRepository.Object);
            mockMapper.Setup(s => s.Map<Join,JoinViewModel>(testData)).Returns(testViewModel);

            var sut = new JoinService(() => mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(testViewModel, result);
        }
    }
}
