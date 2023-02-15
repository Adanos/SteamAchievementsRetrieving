using Newtonsoft.Json;
using SteamAchievementsRetrieving.Models;
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
            string addressApi = _steamAchievementConfiguration.AddressApi;
            _client = new HttpClient
            {
                BaseAddress = new Uri(addressApi)
            };
        }

        public SteamAchievementResponse GetAchievements()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);
            httpValueCollection.Add("?appid", _steamAchievementConfiguration.ApplicationId);
            httpValueCollection.Add("key", _steamAchievementConfiguration.AuthentificationKey);
            httpValueCollection.Add("steamid", _steamAchievementConfiguration.Steamid);
            httpValueCollection.Add("l", _steamAchievementConfiguration.Language);

            var achievements = _client.GetAsync(httpValueCollection.ToString());

            if (achievements.Result.IsSuccessStatusCode)
            {
                var result = achievements.Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SteamAchievementResponse>(result.Result);
            }

            return null;
        }
    }
}
