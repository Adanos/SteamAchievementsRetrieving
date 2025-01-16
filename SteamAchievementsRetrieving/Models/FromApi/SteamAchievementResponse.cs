using System.Net;

namespace SteamAchievementsRetrieving.Models.FromApi
{
    public class SteamAchievementResponse
    {
        public HttpStatusCode StatusCode { get; internal set; }
        public PlayerStats PlayerStats { get; set; }
        public bool Success { get; set; }
    }
}
