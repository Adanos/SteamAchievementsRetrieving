using Newtonsoft.Json;
using SteamAchievementsRetrieving.Models.FromApi;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SteamAchievementsRetrieving
{
    public class SteamAchievementsRetrieving
    {
        private readonly HttpClient _client;
        private readonly SteamAchievementConfiguration _steamAchievementConfiguration;
        public SteamAchievementsRetrieving(SteamAchievementConfiguration steamAchievementConfiguration)
        {
            _steamAchievementConfiguration = steamAchievementConfiguration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_steamAchievementConfiguration.AddressApi)
            };
        }

        public SteamAchievementResponse GetAllAchievements()
        {
            SteamAchievementResponse response = new SteamAchievementResponse();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(Constants.HeaderJsonType));
            var httpValues = HttpUtility.ParseQueryString(string.Empty);
            httpValues.Add(Constants.ApplicationIdWithQuestionMarkParam, _steamAchievementConfiguration.ApplicationId);
            httpValues.Add(Constants.AuthentificationKeyParam, _steamAchievementConfiguration.AuthentificationKey);
            httpValues.Add(Constants.SteamIdParam, _steamAchievementConfiguration.SteamId);
            httpValues.Add(Constants.LanguageParam, _steamAchievementConfiguration.Language);

            var achievements = _client.GetAsync(httpValues.ToString());

            if (achievements.Result.IsSuccessStatusCode)
            {
                var result = achievements.Result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<SteamAchievementResponse>(result.Result);
                response.Success = true;
            }

            return response;
        }
    }
}
