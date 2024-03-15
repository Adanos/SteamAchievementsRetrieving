using System.Collections.Generic;

namespace SteamAchievementsRetrieving.Models.FromApi
{
    public class PlayerStats
    {
        public string SteamId { get; set; }
        public string GameName { get; set; }
        public IList<AchievementResponse> Achievements { get; set; }
        public bool Success { get; set; }
    }
}