using System.Collections.Generic;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CorTabernaclChoir.Tests.Services
{
    [TestClass]
    public class RecordingsServiceTest
    {
        [TestMethod]
        public void Get_ReturnsCorrectModel()
        {
            // Arrange
            var mockSystemVariablesService = new Mock<IAppSettingsService>();
            var mockYouTubeService = new Mock<IYouTubeService>();
            var sut = new RecordingsService(mockSystemVariablesService.Object, mockYouTubeService.Object);

            mockYouTubeService.Setup(s => s.GetVideos(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<Video>());

            // Act
            var result = sut.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Videos.Count);
            Assert.IsInstanceOfType(result, typeof(RecordingsViewModel));
        }
    }
}
