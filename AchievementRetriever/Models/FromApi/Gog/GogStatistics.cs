using System.Collections.Generic;
using AchievementRetriever.Models.FromApi.Steam;

namespace AchievementRetriever.Models.FromApi.Gog;

public class GogStatistics
{
    public IList<AchievementResponse> Achievements { get; set; }
    public GogStat Stat { get; set; }
}