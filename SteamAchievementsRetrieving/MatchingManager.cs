using System.Text.RegularExpressions;

namespace SteamAchievementsRetrieving
{
    public class MatchingManager
    {
        public string FindCountryMatching(string text)
        {
            string countryPattern = @"((\bas\b)|(\bAs\b))\s(\bthe\b\s)?([A-Z][a-z]*\s?)*((,\s[a-zA-Z]*)*\s?\bor\b\s[a-zA-Z\s]*)*";
            string result = null;
            Match match = Regex.Match(text, countryPattern);

            if (match.Success)
            {
                result = match.Captures[0].Value;
                result = result?.Replace("as", "")?.Replace("As", "")?.Replace(" or", ",").Trim();
            }

            return result;
        }
    }
}
