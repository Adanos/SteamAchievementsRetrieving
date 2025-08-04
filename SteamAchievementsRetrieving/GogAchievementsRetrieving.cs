using SteamAchievementsRetrieving.Models.FromApi.Gog;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using SteamAchievementsRetrieving.Models.FromApi;

namespace SteamAchievementsRetrieving
{
    public class GogAchievementsRetrieving : IAchievementsRetrieving
    {
        private readonly HttpClient _httpClient;
        private readonly GogAchievementConfiguration _gogAchievementConfiguration;
        private readonly IParseJsonFromHtml _parseJsonFromHtml;

        public GogAchievementsRetrieving(HttpClient httpClient, IParseJsonFromHtml parseJsonFromHtml, 
            GogAchievementConfiguration gogAchievementConfiguration)
        {
            _httpClient = httpClient;
            _parseJsonFromHtml = parseJsonFromHtml;
            _gogAchievementConfiguration = gogAchievementConfiguration;
        }

        public async Task<AchievementsResponse> GetAllAchievementsAsync()
        {
            if (string.IsNullOrWhiteSpace(_gogAchievementConfiguration.AddressApi) || string.IsNullOrWhiteSpace(_gogAchievementConfiguration.User))
            {
                throw new InvalidOperationException("Invalid configuration for Gog achievements API.");
            }

            var response = new AchievementsResponse();

            try
            {
                var baseAddress = _gogAchievementConfiguration.AddressApi
                    .Replace("{User}", Uri.EscapeDataString(_gogAchievementConfiguration.User))
                    .Replace("{GameId}", Uri.EscapeDataString(_gogAchievementConfiguration.GameId));
                
                _httpClient.BaseAddress = new Uri(baseAddress);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.HeaderJsonType));

                var achievementsResponse = await _httpClient.GetAsync(_httpClient.BaseAddress);

                response.StatusCode = achievementsResponse.StatusCode;

                if (achievementsResponse.IsSuccessStatusCode)
                {
                    var content = await achievementsResponse.Content.ReadAsStringAsync();
                    response = _parseJsonFromHtml.ParseHtml(content);
                }
            }
            catch (UriFormatException ex)
            {
                response.Success = false;
                response.ErrorMessage = $"Request failed: {ex.Message}";
                throw;
            }
            catch (HttpRequestException ex)
            {
                response.Success = false;
                response.ErrorMessage = $"Request failed: {ex.Message}";
                throw;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = $"An error occurred: {ex.Message}";
                throw;
            }

            return response;
        }
    }
}
