using System.Collections.Generic;

namespace SteamAchievementsRetrieving.Models
{
    public class PlayerStats
    {
        public string SteamId { get; set; }
        public string GameName { get; set; }
        public IList<Achievement> Achievements { get; set; }
        public bool Success { get; set; }
    }
}