using SteamAchievementsRetrieving.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteamAchievementsRetrieving
{
    public class AchievementMatrixCreator
    {
        private IList<Achievement> Achievements { get; }
        public IDictionary<string, List<string>> Matrix { get; private set; }
        public AchievementMatrixCreator(IList<Achievement> achievements)
        {
            Achievements = achievements;
            Matrix = new Dictionary<string, List<string>>();
        }

        public void CreateMatrix()
        {
            MatchingManager matchingManager = new MatchingManager();

            foreach (var achievement in Achievements)
            {
                var countries = matchingManager.FindCountryMatching(achievement.Description)?.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim());

                foreach (var country in countries ?? Enumerable.Empty<string>())
                {
                    bool exists = Matrix.TryGetValue(country, out List<string> value);
                    if (exists)
                        value.Add(achievement.Name);
                    else Matrix.Add(country, new List<string>() { achievement.Name });
                }
            }

            Matrix = Matrix.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);

            MergeTheSameKeys();
            Matrix = Matrix.OrderBy(x => x.Key).OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
        }

        private void MergeTheSameKeys()
        {
            IDictionary<string, List<string>> newMatrix = new Dictionary<string, List<string>>();

            var duplicateKeys = Matrix.Keys.Where(x => Regex.Match(x, @"(\bthe\b)|(\bThe\b).*").Success).ToList();

            foreach (var item in duplicateKeys)
            {
                string keyToChange = item.Replace("The ", "").Replace("the ", "");
                Matrix.TryGetValue(item, out List<string> values);
                if (Matrix.ContainsKey(keyToChange))
                {
                    Matrix[keyToChange].AddRange(values);               
                }
                else Matrix.Add(keyToChange, values);
                Matrix.Remove(item);
            }
        }
    }
}
