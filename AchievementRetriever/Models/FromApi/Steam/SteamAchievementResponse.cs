using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AchievementRetriever.Models.FromApi.Steam
{
    public class SteamAchievementResponse : AchievementsResponse
    {
        [JsonPropertyName("playerstats")]
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
