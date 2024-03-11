using NUnit.Framework;
using SteamAchievementsRetrieving;
using SteamAchievementsRetrieving.Models.FromApi;
using System.Collections.Generic;

namespace SteamAchievementsRetrievingTests
{
    class AchievementMatrixCreatorTests
    {
        private IList<Achievement> Achievements { get; set; }
        [SetUp]
        public void Setup()
        {
            Achievements = new List<Achievement>()
            {
                new Achievement() { Name = "Test 1", Description = "As the Knights conquer the world." },
                new Achievement() { Name = "Test 2", Description = "As The Knights have 1000 gold." },
                new Achievement() { Name = "Test 3", Description = "Win a war as Cyprus." },
                new Achievement() { Name = "Test 4", Description = "As the Papacy become emperor." },
                new Achievement() { Name = "Test 5", Description = "Own Jerusalem as the Papal State." }
            };
        }

        [Test]
        public void MergeTheSameKeys_KeysAreSimilarCaseSensitive_ReturnOneKey()
        {
            AchievementMatrixCreator achievementMatrixCreator = new AchievementMatrixCreator(Achievements);
            achievementMatrixCreator.CreateMatrix();
            achievementMatrixCreator.Matrix.TryGetValue("Knights", out List<string> values);

            Assert.IsTrue(achievementMatrixCreator.Matrix.ContainsKey("Knights"));
            Assert.IsFalse(achievementMatrixCreator.Matrix.ContainsKey("the Knights"));
            Assert.IsFalse(achievementMatrixCreator.Matrix.ContainsKey("The Knights"));
            Assert.AreEqual(2, values.Count);
        }

        [Test]
        public void MergeTheSameKeys_DifferentNamesForTheSameCountry_ReturnOneKey()
        {
            AchievementMatrixCreator achievementMatrixCreator = new AchievementMatrixCreator(Achievements);
            achievementMatrixCreator.CreateMatrix();
            achievementMatrixCreator.Matrix.TryGetValue("Papal State", out List<string> values);

            Assert.IsTrue(achievementMatrixCreator.Matrix.ContainsKey("Papal State"));
            Assert.IsFalse(achievementMatrixCreator.Matrix.ContainsKey("Papacy"));
            Assert.AreEqual(2, values.Count);
        }
    }
}
