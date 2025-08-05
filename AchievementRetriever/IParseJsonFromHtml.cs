using AchievementRetriever.Models.FromApi;

namespace AchievementRetriever;

public interface IParseJsonFromHtml
{
    AchievementsResponse ParseHtml(string jsonFromHtml);
}