using System.Net;

namespace SteamAchievementsRetrieving.Models.FromApi.Steam
{
    public class SteamAchievementResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public PlayerStats PlayerStats { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; internal set; }
    }
}
