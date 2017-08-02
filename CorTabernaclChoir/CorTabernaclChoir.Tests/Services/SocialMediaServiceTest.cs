using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Services;
using CorTabernaclChoir.Data.Contracts;
using FluentAssertions;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class SocialMediaServiceTest
    {
        private readonly List<SocialMediaAccount> _socialMediaAccounts = TestData.SocialMediaAccounts();
        private readonly List<ImageFile> _imageFiles = TestData.ImageFiles();

        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<SocialMediaAccount>> _mockSocialMediaRepository;
        private SocialMediaAccount _testAccount;

        [TestInitialize]
        public void Initialise()
        {
            _testAccount = _socialMediaAccounts[10];

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSocialMediaRepository = new Mock<IRepository<SocialMediaAccount>>();

            _mockUnitOfWork.Setup(u => u.Repository<SocialMediaAccount>()).Returns(_mockSocialMediaRepository.Object);
            _mockSocialMediaRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(_testAccount);
            _mockSocialMediaRepository.Setup(r => r.Insert(It.IsAny<SocialMediaAccount>())).Callback<SocialMediaAccount>(sm =>
            {
                _socialMediaAccounts.Add(sm);
                _imageFiles.Add(sm.ImageFile);
            });
        }

        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var sut = new SocialMediaService(() => _mockUnitOfWork.Object);

            // Act
            var result = sut.Get(_testAccount.Id);

            // Assert
            result.Id.Should().Be(_testAccount.Id);
            result.Name.Should().Be(_testAccount.Name);
            result.Url.Should().Be(_testAccount.Url);
            result.ImageFileId.Should().Be(_testAccount.ImageFileId);
        }

        [TestMethod]
        public void Add_InsertsCorrectModel()
        {
            // Arrange
            var model = new SocialMediaViewModel
            {
                Name = "Facebook",
                Url = "https://www.facebook.com/"
            };
            var logo = new ImageFile
            {
                File = Encoding.ASCII.GetBytes("teststring"),
                ContentType = "image/png"
            };
            var sut = new SocialMediaService(() => _mockUnitOfWork.Object);

            // Act
            sut.Add(model, logo);

            // Assert
            _mockSocialMediaRepository.Verify(r => r.Insert(It.IsAny<SocialMediaAccount>()), Times.Once);
            _socialMediaAccounts.Last().Id.Should().Be(0);
            _socialMediaAccounts.Last().Name.Should().Be(model.Name);
            _socialMediaAccounts.Last().Url.Should().Be(model.Url);
            _imageFiles.Last().Id.Should().Be(0);
            _imageFiles.Last().File.ShouldBeEquivalentTo(logo.File);
            _imageFiles.Last().ContentType.Should().Be(logo.ContentType);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void Edit_UpdatesCorrectModel()
        {
            // Arrange
            var model = new SocialMediaViewModel
            {
                Name = _testAccount.Name + "test",
                Url = _testAccount.Url + "/something-else"
            };
            var sut = new SocialMediaService(() => _mockUnitOfWork.Object);

            // Act
            sut.Edit(model, null);

            // Assert
            _testAccount.Name.Should().Be(model.Name);
            _testAccount.Url.Should().Be(model.Url);
            _testAccount.ImageFileId.Should().Be(_testAccount.Id);
            _testAccount.ImageFile.ShouldBeEquivalentTo(_imageFiles.Single(im => im.Id == _testAccount.Id));
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void Edit_IncludingLogoChange_UpdatesCorrectModel()
        {
            // Arrange
            var model = new SocialMediaViewModel
            {
                Id = _testAccount.Id,
                Name = "Facebook",
                Url = "https://www.facebook.com/"
            };
            var logo = new ImageFile
            {
                File = Encoding.ASCII.GetBytes("test string"),
                ContentType = "image/png"
            };
            var sut = new SocialMediaService(() => _mockUnitOfWork.Object);

            // Act
            sut.Edit(model, logo);

            // Assert
            _testAccount.Name.Should().Be(model.Name);
            _testAccount.Url.Should().Be(model.Url);
            _testAccount.ImageFile.Should().Be(logo);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }
    }
}
