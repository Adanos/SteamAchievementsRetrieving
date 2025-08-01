using System.Collections.Generic;
using SteamAchievementsRetrieving.Models;

namespace SteamAchievementsRetrieving.JsonParsers;

public interface IAchievementParserDispatcher
{
    IList<GameAchievement> Parse(string json);
}