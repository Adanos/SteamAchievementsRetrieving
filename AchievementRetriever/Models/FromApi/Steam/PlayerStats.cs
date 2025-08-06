using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AchievementRetriever.Models.FromApi.Steam
{
    public class PlayerStats
    {
        [JsonPropertyName("achievements")]
        public IList<AchievementResponse> Achievements { get; set; }
        [JsonPropertyName("gameName")]
        public string GameName { get; set; }
        public string SteamId { get; set; }
        public bool Success { get; set; }
    }
}