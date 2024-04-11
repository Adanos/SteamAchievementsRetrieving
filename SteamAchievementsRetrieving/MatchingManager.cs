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

        public string FindCountryMatching(string text)
        {
            string countryPattern = @"((\bas\b)|(\bAs\b))\s(\beither\b\s)?(\bthe\b\s)?([A-Z][a-zà-ü]*]*\s?)*((,(\s\bthe\b)?(\s[A-Z][a-zà-ü]*]*)*)*\s?\bor\b(\s(\bthe\b\s)?[A-Z][a-zà-ü]*]*)*)*";
            string result = null;
            Match match = Regex.Match(text, countryPattern);

            if (match.Success)
            {
                result = match.Captures[0].Value;
                result = result?.Replace("as ", "")?.Replace("As ", "")?.Replace(" or", ",")?.Replace("either ", "")?.Replace(", the Emperor", "")?.Replace(" the Emperor", "")
                    ?.Replace(", Emperor", "")?.Replace(" Emperor", "")?.Trim();
            }

            return result;
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
