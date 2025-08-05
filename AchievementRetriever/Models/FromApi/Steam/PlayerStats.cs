using System.Collections.Generic;

namespace AchievementRetriever.Models.FromApi.Steam
{
    public class PlayerStats
    {
        public IList<AchievementResponse> Achievements { get; set; }
        public string GameName { get; set; }
        public string SteamId { get; set; }
        public bool Success { get; set; }
    }
}