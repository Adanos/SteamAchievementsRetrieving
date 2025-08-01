using System.Collections.Generic;
using System.Text.Json;
using SteamAchievementsRetrieving.Models;

namespace SteamAchievementsRetrieving.JsonParsers;

public interface IAchievementParser
{
    bool CanParse(JsonElement root);
    List<GameAchievement> Parse(JsonElement root);
}
