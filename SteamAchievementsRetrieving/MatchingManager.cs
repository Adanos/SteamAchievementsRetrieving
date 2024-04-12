using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SteamAchievementsRetrieving
{
    public class MatchingManager
    {
        private readonly IDictionary<string, string> PhrasesReferringToCountry = new Dictionary<string, string>
        {
            { "English and British missions", "England" },
            { "Mewar Mission Tree", "Mewar" },
            { "Spanish Mission Tree", "Spain" },
            { "Japanese Daimyo", "Japan" },
            { "Russian nation", "Russia" },
            { "Persian mission tree", "Persia" },
        };

        private readonly IDictionary<string, string> Replacements = new Dictionary<string, string>
        {
            { "as ", "" },
            { "As ", "" },
            { " or", "," },
            { "either ", "" },
            { ", the Emperor", "" },
            { " the Emperor", "" },
            { ", Emperor", "" },
            { " Emperor", "" }
        };

        public string FindCountryMatching(string text)
        {
            string letterPattern = "[A-Z][a-zà-ü]*";
            string countryPattern = @$"(\b[aA]s\b)\s(\beither\b\s)?(\bthe\b\s)?({letterPattern}\s?)*((,(\s\bthe\b)?(\s{letterPattern})*)*\s?\bor\b(\s(\bthe\b\s)?{letterPattern})*)*";
            string result = null;
            Match match = Regex.Match(text, countryPattern);

            if (match.Success)
            {
                result = match.Captures[0].Value;
                foreach (var replacement in Replacements)
                    result = Regex.Replace(result, replacement.Key, replacement.Value);
            }

            return result?.Trim();
        }

        public string FindPhrasesReferringToCountry(string text)
        {
            foreach (var item in PhrasesReferringToCountry)
            {
                if (text.Contains(item.Key))
                    return item.Value;
            }
            return null;
        }
    }
}
