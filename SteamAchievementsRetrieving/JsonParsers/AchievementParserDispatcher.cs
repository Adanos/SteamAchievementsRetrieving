using System;
using System.Collections.Generic;
using System.Text.Json;
using SteamAchievementsRetrieving.Models;

namespace SteamAchievementsRetrieving.JsonParsers;

public class AchievementParserDispatcher : IAchievementParserDispatcher
{
    private readonly IList<IAchievementParser> _parsers;

    public AchievementParserDispatcher()
    {
        _parsers = new List<IAchievementParser>
        {
            new SteamAchievementParser(),
            new GogAchievementParser()
        };
    }

    public IList<GameAchievement> Parse(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        foreach (var parser in _parsers)
        {
            if (parser.CanParse(root))
                return parser.Parse(root);
        }

        throw new NotSupportedException("Unsupported JSON format.");
    }
}
