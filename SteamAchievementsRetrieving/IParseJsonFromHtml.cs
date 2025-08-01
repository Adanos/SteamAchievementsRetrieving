using SteamAchievementsRetrieving.Models.FromApi;

namespace SteamAchievementsRetrieving;

public interface IParseJsonFromHtml
{
    AchievementsResponse ParseHtml(string jsonFromHtml);
}