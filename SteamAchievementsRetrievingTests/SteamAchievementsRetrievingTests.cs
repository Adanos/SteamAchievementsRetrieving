using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using SteamAchievementsRetrieving.Models.FromApi.Steam;

namespace SteamAchievementsRetrievingTests
{
    [TestFixture]
    public class SteamAchievementsRetrievingTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private SteamAchievementConfiguration _configurationMock;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationMock = new SteamAchievementConfiguration
            {
                AddressApi = "https://api.steampowered.com",
                ApplicationId = "12345",
                AuthentificationKey = "apikey",
                SteamId = "1234567890",
                Language = "en"
            };
        }

        [Test]
        public async Task GetAllAchievementsAsync_ReturnsSuccessResponse()
        {
            // Arrange
            var expectedResponse = new SteamAchievementResponse
            {
                Success = true,
                StatusCode = HttpStatusCode.OK
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
                    Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
                });

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var service = new SteamAchievementsRetrieving.SteamAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _configurationMock);

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

            var service = new SteamAchievementsRetrieving.SteamAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _configurationMock);

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
            var service = new SteamAchievementsRetrieving.SteamAchievementsRetrieving(_httpClientFactoryMock.Object.CreateClient(), _configurationMock);

            // Act & Assert
            Assert.That(
                service.GetAllAchievementsAsync,
                Throws.TypeOf<InvalidOperationException>()
            );
        }
    }
}