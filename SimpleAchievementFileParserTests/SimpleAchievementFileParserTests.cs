using NUnit.Framework;
using SimpleAchievementFileParser.Model;

namespace SimpleAchievementFileParserTests
{
    public class SimpleAchievementFileParserTests
    {
        [Test]
        public void SimpleAchievementFileParser_ParseFileWithOnlyCommentary_ReturnObject()
        {
            SimpleAchievementFileParser.SimpleAchievementFileParser simpleParser = new SimpleAchievementFileParser.SimpleAchievementFileParser("FileCaseTests\\achievementOnlyCommentaryTest.txt");

            var result = simpleParser.ParseFile();
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void SimpleAchievementFileParser_ParseSimpleFile_ReturnObject()
        {
            SimpleAchievementFileParser.SimpleAchievementFileParser simpleParser = new SimpleAchievementFileParser.SimpleAchievementFileParser("FileCaseTests\\achievementTest.txt");

            var result = simpleParser.ParseFile();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(15, result.FirstOrDefault()?.Id);
            Assert.AreEqual("NEW_ACHIEVEMENT_1_2", result.FirstOrDefault()?.Localization);
        }

        [Test]
        public void SimpleAchievementFileParser_ParseFile_ReturnObject()
        {
            SimpleAchievementFileParser.SimpleAchievementFileParser simpleParser = new SimpleAchievementFileParser.SimpleAchievementFileParser("FileCaseTests\\achievementTest2.txt");

            var result = simpleParser.ParseFile();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(17, result.FirstOrDefault()?.Id);
            Assert.AreEqual("NEW_ACHIEVEMENT_7_2", result.FirstOrDefault()?.Localization);
        }

        [Test]
        public void SimpleAchievementFileParser_CreateAchievements_ReturnObject()
        {
            var fileStream = new FileStream("QueueCaseTests\\QueueCaseTest2.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            string line = reader.ReadLine();

            Queue<string> queue = new Queue<string>();
            while ((line = reader.ReadLine()) != null)
            {
                queue.Enqueue(line);
            }

            SimpleAchievementFileParser.SimpleAchievementFileParser simpleParser = new SimpleAchievementFileParser.SimpleAchievementFileParser("FileCaseTests\\achievementTest2.txt");
            Achievement result = new Achievement() { Id = 17, Localization = "NEW_ACHIEVEMENT_7_2" };
            result = simpleParser.CreateAchievements(result, queue) as Achievement;
            Assert.AreEqual(17, result.Id);
            Assert.AreEqual("NEW_ACHIEVEMENT_7_2", result.Localization);
            Assert.IsNotNull(result.Possible.NotModel);
            Assert.IsNotNull(result.Possible.HasOneOfDlc);
            Assert.AreEqual(2, result.Possible.HasOneOfDlc.Names.Count);
        }
    }
}
