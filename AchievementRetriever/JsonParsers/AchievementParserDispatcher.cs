using System;
using System.Collections.Generic;
using System.Text.Json;
using AchievementRetriever.Models;

namespace AchievementRetriever.JsonParsers;

public class AchievementParserDispatcher : IAchievementParserDispatcher
{
    private readonly IAchievementParser _activeParser;

    public AchievementParserDispatcher(AchievementSourceConfiguration achievementSourceConfiguration)
    {
        IDictionary<AchievementSource, IAchievementParser> parsers = new Dictionary<AchievementSource, IAchievementParser>
        {
            { AchievementSource.Steam, new SteamAchievementParser() },
            { AchievementSource.GoG, new GogAchievementParser() }
        };
        
        var source = achievementSourceConfiguration.Name;

        if (!parsers.TryGetValue(source, out _activeParser))
        {
            var available = string.Join(", ", parsers.Keys);
            throw new InvalidOperationException($"Invalid parser '{source}'. Available: {available}");
        }
    }

    public IList<GameAchievement> Parse(string json)
    {
        using var doc = JsonDocument.Parse(json);
        return _activeParser.Parse(doc.RootElement);
    }
}
