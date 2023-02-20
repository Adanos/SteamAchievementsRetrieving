using NUnit.Framework;
using SteamAchievementsRetrieving;

namespace SteamAchievementsRetrievingTests
{
    public class MatchingManagerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Example text without matching pattern.", null)]
        [TestCase("As Aztecs, conquer the world.", "Aztecs")]
        [TestCase("Become the military hegemon as Spain.", "Spain")]
        [TestCase("Have 10000 ducats as Venice, owning less than 10 cities.", "Venice")]
        [TestCase("Conquer Greece as Cyprus or The Knights.", "Cyprus, The Knights")]
        [TestCase("As Kazan or Nogai, own all Siberia.", "Kazan, Nogai")]
        [TestCase("As Tver, Rostov or Odoyev, conquer all Russia.", "Tver, Rostov, Odoyev")]
        [TestCase("As Tver, Rostov, Kazan or Odoyev, conquer all Russia.", "Tver, Rostov, Kazan, Odoyev")]
        [TestCase("As Tver, Rostov, Kazan, Crimea or Odoyev, conquer all Russia.", "Tver, Rostov, Kazan, Crimea, Odoyev")]
        [TestCase("Conquer all Russia as Tver, Rostov, Kazan, Crimea or Odoyev.", "Tver, Rostov, Kazan, Crimea, Odoyev")]
        [TestCase("As Spain have Paris, Roma, Berlin under you.", "Spain")]
        [TestCase("As the Papacy own Jerusalem.", "the Papacy")]
        [TestCase("As Perm, own or have a subject own the Russian, Siberian and Cascadian Regions.", "Perm")]
        [TestCase("Form Bavaria starting as München and win with Dortmund and Berlin.", "München")]
        public void FindMatching_ExistSubstring_ReturnState(string text, string expectedResult)
        {
            MatchingManager matchingManager = new MatchingManager();
            string result = matchingManager.FindCountryMatching(text);
            Assert.AreEqual(expectedResult, result);
        }
    }
}