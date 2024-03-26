using NUnit.Framework;
using SimpleAchievementFileParser.Model;

namespace SimpleAchievementFileParserTests
{
    public class AchievementsStructureFileParserTests
    {
        [Test]
        public void AchievementsStructureFileParser_ParseFileWithOnlyCommentary_ReturnObject()
        {
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\achievementOnlyCommentaryTest.txt");

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseSimpleFile_ReturnObject()
        {
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\achievementTest.txt");

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(15));
            Assert.That(result.FirstOrDefault()?.Localization, Is.EqualTo("NEW_ACHIEVEMENT_1_2"));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFile_ReturnObject()
        {
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\achievementTest2.txt");

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(17));
            Assert.That(result.FirstOrDefault()?.Localization, Is.EqualTo("NEW_ACHIEVEMENT_7_2"));
        }

        [Test]
        public void AchievementsStructureFileParser_CreateAchievements_ReturnObject()
        {
            var fileStream = new FileStream("QueueCaseTests\\QueueCaseTest2.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            string line = reader.ReadLine();

            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                queue.Enqueue(new KeyValuePair<string, string>(values[0], values[1] ?? null));
            }

            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\achievementTest2.txt");
            Achievement result = new Achievement() { Id = 17, Localization = "NEW_ACHIEVEMENT_7_2" };
            result = simpleParser.CreateAchievements(result, queue) as Achievement;
            Assert.That(result?.Id, Is.EqualTo(17));
            Assert.That(result?.Localization, Is.EqualTo("NEW_ACHIEVEMENT_7_2"));
            Assert.That(result?.Possible?.NotModel, Is.Not.Null);
            Assert.That(result?.Possible?.HasOneOfDlc, Is.Not.Null);
            Assert.That(result?.Possible?.HasOneOfDlc?.Names.Count, Is.EqualTo(2));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithTwoOrsInVisible_ReturnObject()
        {
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\achievementTwoOrsInVisibleTest.txt");

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(26));
            Assert.That(result.FirstOrDefault()?.Localization, Is.EqualTo("NEW_ACHIEVEMENT_4_8"));
            Assert.That(result.FirstOrDefault()?.VisibleRequirements?.HasOneOfDlc, Is.Not.Null);
            var dlcNames = result.FirstOrDefault()?.VisibleRequirements?.HasOneOfDlc?.Names;
            Assert.That(dlcNames?.Count, Is.EqualTo(2));
            Assert.That(dlcNames?.FirstOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc4"));
            Assert.That(dlcNames?.LastOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc2"));
        }
    }
}
