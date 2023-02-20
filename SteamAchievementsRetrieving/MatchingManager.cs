using System.Text.RegularExpressions;

namespace SteamAchievementsRetrieving
{
    public class MatchingManager
    {
        public string FindCountryMatching(string text)
        {
            string countryPattern = @"((\bas\b)|(\bAs\b))\s(\bthe\b\s)?([A-Z][a-zà-ü]*]*\s?)*((,(\s[A-Z][a-zà-ü]*]*)*)*\s?\bor\b(\s[A-Z][a-zà-ü]*]*)*)*";
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
