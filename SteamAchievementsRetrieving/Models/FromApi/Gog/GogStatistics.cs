using System.Collections.Generic;

namespace SteamAchievementsRetrieving.Models.FromApi.Gog;

public class GogStatistics
{
    public IList<GogAchievement> Achievements { get; set; }
    public GogStat Stat { get; set; }
}