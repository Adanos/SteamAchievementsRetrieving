using System.Net;

namespace SteamAchievementsRetrieving.Models.FromApi
{
    public class GogAchievementResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; internal set; }
    }
}
