using SteamAchievementsRetrieving.Models;
using System.Collections.Generic;
using System.Linq;

namespace SteamAchievementsRetrieving.Services
{
    public class AchievementManager
    {
        private IList<Achievement> Achievements { get; }

        public AchievementManager(IList<Achievement> achievements)
        {
            Achievements = achievements;
        }

        public IList<Achievement> GetUnlockedAchievements()
        {
            return Achievements.Where(achievement => achievement.Achieved).ToList();
        }

        public IList<Achievement> GetLockedAchievements()
        {
            return Achievements.Where(achievement => !achievement.Achieved).ToList();
        }
    }
}
