using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SteamAchievementsRetrieving.Models;

namespace SteamAchievementsRetrieving.JsonParsers;

public class GogAchievementParser : IAchievementParser
{
    private const string Achievement = "achievement";
    private const string Description = "description";
    private const string Name = "name";
    private const string IsUnlocked = "isUnlocked";
    private const string Stats = "stats";

    public bool CanParse(JsonElement root) =>
        root.ValueKind == JsonValueKind.Array &&
        root.EnumerateArray().First().TryGetProperty(Achievement, out _);

    public List<GameAchievement> Parse(JsonElement root)
    {
        var results = new List<GameAchievement>();

        foreach (var item in root.EnumerateArray())
        {
            var achievement = item.GetProperty(Achievement);
            var name = achievement.GetProperty(Name).GetString();
            var description = achievement.GetProperty(Description).GetString();

            bool? isUnlocked = null;
            if (item.TryGetProperty(Stats, out var stats))
            {
                foreach (var stat in stats.EnumerateObject())
                {
                    if (stat.Value.TryGetProperty(IsUnlocked, out var unlock))
                    {
                        isUnlocked = unlock.GetBoolean();
                        break;
                    }
                }
            }

            results.Add(new GameAchievement
            {
                Name = name,
                Description = description,
                IsUnlocked = isUnlocked
            });
        }

        return results;
    }
}
