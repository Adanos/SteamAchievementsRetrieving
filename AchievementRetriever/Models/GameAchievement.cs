namespace AchievementRetriever.Models;

public class GameAchievement
{
    public string GameName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool? IsUnlocked { get; set; }
}