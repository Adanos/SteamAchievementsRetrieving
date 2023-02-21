using NUnit.Framework;
using SteamAchievementsRetrieving;
using SteamAchievementsRetrieving.Models;
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
                new Achievement() { Name = "Test 3", Description = "Win a war as Cyprus." }
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
    }
}
