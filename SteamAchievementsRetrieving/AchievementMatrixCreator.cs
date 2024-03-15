using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Models.FromApi;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteamAchievementsRetrieving
{
    public class AchievementMatrixCreator
    {
        private readonly IDictionary<string, string> CountryNamesReplacement = new Dictionary<string, string>
        {
            { "Papacy", "Papal State" }
        };

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
                var additionalCountry = matchingManager.FindPhrasesReferringToCountry(achievement.Description);
                
                foreach (var country in countries ?? Enumerable.Empty<string>())
                {
                    UpdateMatrixByCountry(achievement, country);
                }

                if (!string.IsNullOrEmpty(additionalCountry))
                    UpdateMatrixByCountry(achievement, additionalCountry);
            }

            Matrix = Matrix.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);

            MergeTheSameKeys();
            Matrix = Matrix.OrderBy(x => x.Key).OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
        }

        private void UpdateMatrixByCountry(AchievementResponse achievement, string country)
        {
            bool exists = Matrix.TryGetValue(country, out List<string> value);
            if (exists)
                value.Add(achievement.Name);
            else Matrix.Add(country, new List<string>() { achievement.Name });
        }

        private void MergeTheSameKeys()
        {
            var duplicateKeys = Matrix.Keys.Where(x => Regex.Match(x, @"(\bthe\b)|(\bThe\b).*").Success).ToList();

            foreach (var item in duplicateKeys)
            {
                string keyToChange = item.Replace("The ", "").Replace("the ", "");
                keyToChange = CountryNamesReplacement.ContainsKey(keyToChange) ? CountryNamesReplacement[keyToChange] : keyToChange;
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
