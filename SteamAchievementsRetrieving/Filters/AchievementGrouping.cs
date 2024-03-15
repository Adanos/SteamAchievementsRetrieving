using SteamAchievementsRetrieving.Models.FromApi;
using System.Collections.Generic;
using System.Linq;

namespace SteamAchievementsRetrieving.Services
{
    public class AchievementGrouping
    {
        private IList<AchievementResponse> Achievements { get; }

        public AchievementGrouping(IList<AchievementResponse> achievements)
        {
            Achievements = achievements;
        }

        public IList<AchievementResponse> GetUnlockedAchievements()
        {
            return Achievements.Where(achievement => achievement.Achieved).ToList();
        }

        public IList<AchievementResponse> GetLockedAchievements()
        {
            return Achievements.Where(achievement => !achievement.Achieved).ToList();
        }
    }
}
