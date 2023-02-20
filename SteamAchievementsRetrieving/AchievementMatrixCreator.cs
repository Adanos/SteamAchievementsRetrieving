using SteamAchievementsRetrieving.Models;
using System.Collections.Generic;
using System.Linq;

namespace SteamAchievementsRetrieving
{
    public class AchievementMatrixCreator
    {
        private IList<Achievement> Achievements { get; }
        private IDictionary<string, List<string>> matrix = new Dictionary<string, List<string>>();
        public AchievementMatrixCreator(IList<Achievement> achievements)
        {
            Achievements = achievements;
        }

        public void CreateMatrix()
        {
            MatchingManager matchingManager = new MatchingManager();

            foreach (var achievement in Achievements)
            {
                var countries = matchingManager.FindCountryMatching(achievement.Description)?.Split(",").Where(x => !string.IsNullOrEmpty(x));

                foreach (var country in countries ?? Enumerable.Empty<string>())
                {
                    bool exists = matrix.TryGetValue(country, out List<string> value);
                    if (exists)
                        value.Add(achievement.Name);
                    else matrix.Add(country, new List<string>() { achievement.Name });
                }
            }
        }
    }
}
