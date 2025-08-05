using System.Collections.Generic;
using AchievementRetriever.Models;

namespace AchievementRetriever.JsonParsers;

public interface IAchievementParserDispatcher
{
    IList<GameAchievement> Parse(string json);
}