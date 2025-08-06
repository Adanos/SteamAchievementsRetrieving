using System.Collections.Generic;
using System.Text.Json;
using AchievementRetriever.Models;

namespace AchievementRetriever.JsonParsers;

public class SteamAchievementParser : IAchievementParser
{
    private const string PlayerStats = "playerstats";
    private const string GameName = "gameName";
    private const string Achievements = "achievements";
    private const string Name = "name";
    private const string Description = "description";
    private const string Achieved = "achieved";

    public bool CanParse(JsonElement root) =>
        root.ValueKind == JsonValueKind.Object && root.TryGetProperty(PlayerStats, out _);

    public IList<GameAchievement> Parse(string json)
    {
        using var doc = JsonDocument.Parse(json);
        return Parse(doc.RootElement);
    }
    
    private List<GameAchievement> Parse(JsonElement root)
    {
        var results = new List<GameAchievement>();
        var playerStats = root.GetProperty(PlayerStats);

        string gameName = playerStats.GetProperty(GameName).GetString();

        if (playerStats.TryGetProperty(Achievements, out var achievements) &&
            achievements.ValueKind == JsonValueKind.Array)
        {
            foreach (var achievement in achievements.EnumerateArray())
            {
                results.Add(new GameAchievement
                {
                    GameName = gameName,
                    Name = achievement.GetProperty(Name).GetString(),
                    Description = achievement.GetProperty(Description).GetString(),
                    IsUnlocked = achievement.GetProperty(Achieved).GetInt32() == 1
                });
            }
        }

        return results;
    }
}
