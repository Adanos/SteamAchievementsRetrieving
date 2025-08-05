using System.Collections.Generic;
using System.Net;

namespace AchievementRetriever.Models.FromApi;

public class AchievementsResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; internal set; }
    public IList<GameAchievement> Achievements { get; set; }
}