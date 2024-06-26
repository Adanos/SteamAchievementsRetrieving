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
        [TestCase("Form Bavaria starting as M�nchen and win with Dortmund and Berlin.", "M�nchen")]
        [TestCase("As Asturias, drink a rum.", "Asturias")]
        [TestCase("Starting as either Russia or France, own Rome.", "Russia, France")]
        [TestCase("Starting as either the Livonian Order or the Teutonic Order, own Rome.", "the Livonian Order, the Teutonic Order")]
        [TestCase("Starting as either the Livonian Order or the Emperor, own Rome.", "the Livonian Order")]
        [TestCase("As the Papacy, the Livonian Order, the Emperor or the Knights own Jerusalem.", "the Papacy, the Livonian Order, the Knights")]
        public void FindMatching_ExistSubstring_ReturnState(string text, string expectedValue)
        {
            MatchingManager matchingManager = new MatchingManager();
            string result = matchingManager.FindCountryMatching(text);
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [TestCase("Example text.", null)]
        [TestCase("Complete the Spanish Mission Tree.", "Spain")]
        [TestCase("Complete the Mewar Mission Tree.", "Mewar")]
        public void PhrasesReferringToCountry_DifferentNamesForTheSameCountry_ReturnState(string description, string expectedValue)
        {
            MatchingManager matchingManager = new MatchingManager();

            string result = matchingManager.FindPhrasesReferringToCountry(description);
            Assert.That(result, Is.EqualTo(expectedValue));
        }
    }
}