using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using AchievementRetriever.JsonParsers;
using AchievementRetriever.Models;
using AchievementRetriever.Models.FromApi.Steam;

namespace AchievementRetrieverTests
{
    [TestFixture]
    public class SteamAchievementsRetrievingTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<IAchievementParserDispatcher> _achievementParserDispatcherMock;
        private SteamAchievementConfiguration _configurationMock;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _achievementParserDispatcherMock = new Mock<IAchievementParserDispatcher>();
            _configurationMock = new SteamAchievementConfiguration
            {
                AddressApi = "https://api.steampowered.com",
                ApplicationId = "12345",
                AuthentificationKey = "apikey",
                SteamId = "1234567890",
                Language = "en"
            };
            _achievementParserDispatcherMock
                .Setup(x => x.GetParser())
                .Returns(new SteamAchievementParser());
        }

        [Test]
        public async Task GetAllAchievementsAsync_ReturnsSuccessResponse()
        {
            // Arrange
            var expectedResponse = new SteamAchievementResponse
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                PlayerStats = new PlayerStats() { GameName = "gameName", SteamId = "1", Achievements = new List<AchievementResponse>()}
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
                    Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var service = new AchievementRetriever.SteamAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _achievementParserDispatcherMock.Object, _configurationMock);

            // Act
            var result = await service.GetAllAchievementsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetAllAchievementsAsync_ReturnsErrorResponse_OnFailure()
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

            var service = new AchievementRetriever.SteamAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _achievementParserDispatcherMock.Object, _configurationMock);

            // Act
            var result = await service.GetAllAchievementsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void GetAllAchievementsAsync_ThrowsException_OnInvalidConfiguration()
        {
            // Arrange
            _configurationMock.AddressApi = null; // Simulate invalid configuration
            var service = new AchievementRetriever.SteamAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _achievementParserDispatcherMock.Object, _configurationMock);

            // Act & Assert
            Assert.That(
                service.GetAllAchievementsAsync,
                Throws.TypeOf<InvalidOperationException>()
            );
        }
    }
}