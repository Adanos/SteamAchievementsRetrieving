﻿using Newtonsoft.Json;
using SteamAchievementsRetrieving.Models.FromApi.Steam;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace SteamAchievementsRetrieving
{
    public class SteamAchievementsRetrieving
    {
        private readonly HttpClient _httpClient;
        private readonly SteamAchievementConfiguration _steamAchievementConfiguration;
        public SteamAchievementsRetrieving(HttpClient httpClient, SteamAchievementConfiguration steamAchievementConfiguration)
        {
            _httpClient = httpClient;
            _steamAchievementConfiguration = steamAchievementConfiguration;
        }

        public async Task<SteamAchievementResponse> GetAllAchievementsAsync()
        {
            var response = new SteamAchievementResponse();

            if (string.IsNullOrWhiteSpace(_steamAchievementConfiguration.AddressApi) || string.IsNullOrWhiteSpace(_steamAchievementConfiguration.ApplicationId) ||
                string.IsNullOrWhiteSpace(_steamAchievementConfiguration.AuthentificationKey) || string.IsNullOrWhiteSpace(_steamAchievementConfiguration.SteamId) ||
                string.IsNullOrWhiteSpace(_steamAchievementConfiguration.Language))
            {
                throw new InvalidOperationException("Invalid Steam achievement configuration.");
            }

            try
            {
                var uriBuilder = new UriBuilder(_steamAchievementConfiguration.AddressApi);
                var query = HttpUtility.ParseQueryString(string.Empty);
                query[Constants.ApplicationIdWithQuestionMarkParam] = _steamAchievementConfiguration.ApplicationId;
                query[Constants.AuthentificationKeyParam] = _steamAchievementConfiguration.AuthentificationKey;
                query[Constants.SteamIdParam] = _steamAchievementConfiguration.SteamId;
                query[Constants.LanguageParam] = _steamAchievementConfiguration.Language;
                uriBuilder.Query = query.ToString();

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.HeaderJsonType));

                var achievementsResponse = await _httpClient.GetAsync(uriBuilder.Uri);

                response.StatusCode = achievementsResponse.StatusCode;

                if (achievementsResponse.IsSuccessStatusCode)
                {
                    var content = await achievementsResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<SteamAchievementResponse>(content);
                    response.Success = true;
                }
            }
            catch (HttpRequestException ex)
            {
                response.Success = false;
                response.ErrorMessage = $"HTTP request failed: {ex.Message}";
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
