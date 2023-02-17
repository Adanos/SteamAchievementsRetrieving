using System.Text.RegularExpressions;

namespace SteamAchievementsRetrieving
{
    public class MatchingManager
    {
        public string FindCountryMatching(string text)
        {
            string countryPattern = @"\bas\b\s[a-z]*(((,\s[a-z]*)*\s\bor\b)?\s[a-z\s]*)?";
            string result = null;
            Match match = Regex.Match(text, countryPattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                result = match.Captures[0].Value;
                result = result?.Replace("as", "")?.Replace("As", "")?.Replace(" or", ",").Trim();
            }

            return result;
        }
    }
}
