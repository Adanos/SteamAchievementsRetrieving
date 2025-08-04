using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using SteamAchievementsRetrieving;
using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Models.FromApi;
using SteamAchievementsRetrieving.Models.FromApi.Gog;

namespace SteamAchievementsRetrievingTests
{
    [TestFixture]
    public class GogAchievementServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private GogAchievementConfiguration _configurationMock;
        private Mock<IParseJsonFromHtml> _parseJsonFromHtmlMock;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _parseJsonFromHtmlMock = new Mock<IParseJsonFromHtml>();
            _configurationMock = new GogAchievementConfiguration
            {
                AddressApi = "https://www.gog.com/u/{0}/game/{1}",
                User = "testUser",
                GameId = "12345"
            };
            
            _parseJsonFromHtmlMock
                .Setup(p => p.ParseHtml(It.IsAny<string>()))
                .Returns(new AchievementsResponse
                {
                    Achievements = new List<GameAchievement>
                    {
                        new GameAchievement { Name = "Test", Description = "Desc" }
                    },
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                });
        }

        [Test]
        public async Task GetAllAchievements_ReturnsSuccessResponse()
        {
            // Arrange
            var expectedResponse = new AchievementsResponse
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Achievements = new List<GameAchievement>(){ new GameAchievement() {GameName = "Name", Description = "desc", Name = "name", IsUnlocked = false} }
            };

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        $@"window.profilesData.achievements={JsonConvert.SerializeObject(expectedResponse.Achievements)}")
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var service = new GogAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _parseJsonFromHtmlMock.Object, _configurationMock);

            // Act
            var result = await service.GetAllAchievementsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetAllAchievements_ReturnsErrorResponse_OnFailure()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("")
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var service = new GogAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _parseJsonFromHtmlMock.Object, _configurationMock);

            // Act
            var result = await service.GetAllAchievementsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void GetAllAchievements_ReturnsError_OnInvalidUri()
        {
            // Arrange
            _configurationMock.AddressApi = "invalid-url";
            var service = new GogAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _parseJsonFromHtmlMock.Object, _configurationMock);

            // Act & Assert
            Assert.That(
                service.GetAllAchievementsAsync,
                Throws.TypeOf<UriFormatException>()
            );
        }

        [Test]
        public void GetAllAchievements_ReturnsErrorResponse_OnTimeout()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new TaskCanceledException("Request timed out"));

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var service = new GogAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _parseJsonFromHtmlMock.Object, _configurationMock);

            // Act & Assert
            Assert.That(
                service.GetAllAchievementsAsync,
                Throws.TypeOf<TaskCanceledException>()
            );
        }
    }
}
