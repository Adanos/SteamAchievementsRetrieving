using System.Net;

namespace SteamAchievementsRetrieving.Models.FromApi.Gog
{
    public class GogAchievementResponse
    {
        public GogStatistics Statistics { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; internal set; }
    }
}
