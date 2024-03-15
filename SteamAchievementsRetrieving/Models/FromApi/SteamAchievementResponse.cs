using System.Net;

namespace SteamAchievementsRetrieving.Models.FromApi
{
    public class SteamAchievementResponse
    {
        public PlayerStats PlayerStats { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; internal set; }
    }
}
