using System.Threading.Tasks;
using SteamAchievementsRetrieving.Models.FromApi;

namespace SteamAchievementsRetrieving;

public interface IAchievementsRetrieving
{
    Task<AchievementsResponse> GetAllAchievementsAsync();
}