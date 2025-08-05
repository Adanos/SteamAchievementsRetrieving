using System.Collections.Generic;

namespace AchievementRetriever.Models.FromApi.Steam
{
    public class SteamAchievementResponse : AchievementsResponse
    {
        public PlayerStats PlayerStats { get; set; }
        public IList<AchievementResponse> GetAchievements()
        {
            return PlayerStats.Achievements;
        }

        public string GetGameName()
        {
            return PlayerStats.GameName;
        }
    }
}
