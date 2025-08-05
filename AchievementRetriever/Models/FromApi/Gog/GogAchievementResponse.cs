using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace AchievementRetriever.Models.FromApi.Gog
{
    public class GogAchievementResponse : AchievementsResponse
    {
        [JsonIgnore]
        public GogStat Stat { get; set; }
        [JsonPropertyName("stats")]
        private Dictionary<string, GogStat> StatsDict
        {
            set => Stat = value?.Values.FirstOrDefault();
        }
    }
}
