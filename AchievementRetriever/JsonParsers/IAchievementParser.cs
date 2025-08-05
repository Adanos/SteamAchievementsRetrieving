using System.Collections.Generic;
using System.Text.Json;
using AchievementRetriever.Models;

namespace AchievementRetriever.JsonParsers;

public interface IAchievementParser
{
    bool CanParse(JsonElement root);
    List<GameAchievement> Parse(JsonElement root);
}
