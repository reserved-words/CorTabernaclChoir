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
        private readonly JoinViewModel _testViewModel = new JoinViewModel();
        private readonly Join _testModel = TestData.Join();
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private Mock<IRepository<Join>> _mockRepository;

        private JoinService GetSubjectUnderTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<Join>>();

            _mockRepository.Setup(r => r.GetSingle()).Returns(_testModel);
            _mockUnitOfWork.Setup(u => u.Repository<Join>()).Returns(_mockRepository.Object);
            _mockMapper.Setup(s => s.Map<Join, JoinViewModel>(_testModel)).Returns(_testViewModel);

            return new JoinService(() => _mockUnitOfWork.Object, _mockMapper.Object);
        }

        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.Get();

            // Assert
            Assert.AreEqual(_testViewModel, result);
        }

        [TestMethod]
        public void GetForEdit_ReturnsCorrectModel()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            var result = sut.GetForEdit();

            // Assert
            Assert.AreEqual(_testModel, result);
        }

        [TestMethod]
        public void Save_UpdatesModel()
        {
            // Arrange
            var sut = GetSubjectUnderTest();

            // Act
            sut.Save(_testModel);

            // Assert
            _mockRepository.Verify(r => r.Update(_testModel), Times.Once);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }
    }
}
