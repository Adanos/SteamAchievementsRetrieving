using Newtonsoft.Json;
using SteamAchievementsRetrieving.Models.FromApi;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace SteamAchievementsRetrieving
{
    public class SteamAchievementsRetrieving
    {
        private readonly SteamAchievementConfiguration _steamAchievementConfiguration;
        public SteamAchievementsRetrieving(SteamAchievementConfiguration steamAchievementConfiguration)
        {
            _steamAchievementConfiguration = steamAchievementConfiguration;
        }

        public async Task<SteamAchievementResponse> GetAllAchievements()
        {
            SteamAchievementResponse response = new SteamAchievementResponse();
            using(HttpClient client = new())
            {
                client.BaseAddress = new Uri(_steamAchievementConfiguration.AddressApi);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(Constants.HeaderJsonType));
                var httpValues = HttpUtility.ParseQueryString(string.Empty);
                httpValues.Add(Constants.ApplicationIdWithQuestionMarkParam, _steamAchievementConfiguration.ApplicationId);
                httpValues.Add(Constants.AuthentificationKeyParam, _steamAchievementConfiguration.AuthentificationKey);
                httpValues.Add(Constants.SteamIdParam, _steamAchievementConfiguration.SteamId);
                httpValues.Add(Constants.LanguageParam, _steamAchievementConfiguration.Language);

                var achievements = await client.GetAsync(httpValues.ToString());
                response.StatusCode = achievements.StatusCode;

                if (achievements.IsSuccessStatusCode)
                {
                    var result = achievements.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<SteamAchievementResponse>(result.Result);
                    response.Success = true;
                }
            }
            
            return response;
        }
    }
}
