using System.Collections.Generic;
using SteamAchievementsRetrieving.Models.FromApi.Steam;

namespace SteamAchievementsRetrieving.Models.FromApi.Gog;

public class GogStatistics
{
    public IList<AchievementResponse> Achievements { get; set; }
    public GogStat Stat { get; set; }
}