namespace AchievementRetriever.Models.FromApi.Gog;
using System;
public class GogStat
{
    public bool IsUnlocked { get; set; }
    public DateTime? UnlockDate { get; set; }
    public string GameName { get; set; } = "Europa Universalis IV";
}