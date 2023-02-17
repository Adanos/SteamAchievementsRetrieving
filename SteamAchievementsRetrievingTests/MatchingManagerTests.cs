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

        [TestCase("As Aztecs, conquer the world.", "Aztecs")]
        [TestCase("Become the military hegemon as Spain.", "Spain")]
        [TestCase("Have 10000 ducats as Venice, owning less than 10 cities.", "Venice")]
        [TestCase("Conquer Greece as Cyprus or The Knights.", "Cyprus, The Knights")]
        [TestCase("As Kazan or Nogai, own all Siberia.", "Kazan, Nogai")]
        public void FindMatching_ExistSubstring_ReturnState(string text, string expectedResult)
        {
            MatchingManager matchingManager = new MatchingManager();
            string result = matchingManager.FindMatching(text);
            Assert.AreEqual(expectedResult, result);
        }
    }
}