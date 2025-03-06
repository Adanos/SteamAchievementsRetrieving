using Newtonsoft.Json;
using SteamAchievementsRetrieving.Models.FromApi.Gog;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamAchievementsRetrieving
{
    public class GogAchievementsRetrieving
    {
        private readonly HttpClient _httpClient;
        private readonly GogAchievementConfiguration _gogAchievementConfiguration;
        public GogAchievementsRetrieving(HttpClient httpClient, GogAchievementConfiguration gogAchievementConfiguration)
        {
            _httpClient = httpClient;
            _gogAchievementConfiguration = gogAchievementConfiguration;
        }

        public async Task<GogAchievementResponse> GetAllAchievementsAsync()
        {
            if (string.IsNullOrWhiteSpace(_gogAchievementConfiguration.AddressApi) || string.IsNullOrWhiteSpace(_gogAchievementConfiguration.User) 
                || string.IsNullOrWhiteSpace(_gogAchievementConfiguration.GameId))
            {
                throw new InvalidOperationException("Invalid configuration for Gog achievements API.");
            }

            var response = new GogAchievementResponse();

            try
            {
                var baseAddress = string.Format(_gogAchievementConfiguration.AddressApi, Uri.EscapeDataString(_gogAchievementConfiguration.User),
                    Uri.EscapeDataString(_gogAchievementConfiguration.GameId));

                _httpClient.BaseAddress = new Uri(baseAddress);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.HeaderJsonType));

                var achievementsResponse = await _httpClient.GetAsync(_httpClient.BaseAddress);

                response.StatusCode = achievementsResponse.StatusCode;

                if (achievementsResponse.IsSuccessStatusCode)
                {
                    var content = await achievementsResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<GogAchievementResponse>(content);
                    response.Success = true;
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
