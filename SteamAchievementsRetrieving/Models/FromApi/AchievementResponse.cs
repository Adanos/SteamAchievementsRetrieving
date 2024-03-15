using System.Collections.Generic;

namespace SteamAchievementsRetrieving.Models.FromApi
{
    public class AchievementResponse
    {
        public bool Achieved { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
