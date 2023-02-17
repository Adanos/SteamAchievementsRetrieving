using System.Text.RegularExpressions;

namespace SteamAchievementsRetrieving
{
    public class MatchingManager
    {
        public string FindMatching(string text)
        {
            string pattern = @"\bas\b\s[a-zA-Z]*(\s\bor\b[a-zA-Z\s]*)?";
            string result = null;

            Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                result = match.Captures[0].Value;
                result = result?.Replace("as", "")?.Replace("As", "")?.Replace(" or", ",").Trim();
            }

            return result;
        }
    }
}
