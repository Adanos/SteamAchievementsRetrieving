using NUnit.Framework;

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
            Assert.That(result.FirstOrDefault()?.Possible?.NormalProvinceValues, Is.EqualTo("yes"));
            Assert.That(result.FirstOrDefault()?.Possible?.NormalOrHistoricalNations, Is.EqualTo("yes"));
        }

        [Test]
        public void AchievementsStructureFileParser_CreateAchievements_ReturnObject()
        {
            var fileStream = new FileStream("QueueCaseTests\\QueueCaseTest2.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            string? line = string.Empty;

            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                queue.Enqueue(new KeyValuePair<string, string>(values[0], values[1]));
            }

            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\achievementTest2.txt");

            var result = simpleParser.CreateAchievements(queue);
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(17));
            Assert.That(result.FirstOrDefault()?.Localization, Is.EqualTo("NEW_ACHIEVEMENT_7_2"));
            Assert.That(result.FirstOrDefault()?.Possible?.NotModel, Is.Not.Null);
            Assert.That(result.FirstOrDefault()?.Possible?.HasOneOfDlc, Is.Not.Null);
            Assert.That(result.FirstOrDefault()?.Possible?.HasOneOfDlc?.Names.Count, Is.EqualTo(2));
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

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithTokenProvinceId_ReturnObject()
        {
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\achievementWithProvinceIdTest.txt");

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault()?.Localization, Is.EqualTo("NEW_ACHIEVEMENT_0_1"));
            Assert.That(result.FirstOrDefault()?.UnspecifiedNodes?.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.UnspecifiedNodes?.FirstOrDefault()?.NodeName, Is.EqualTo("provinces_to_highlight"));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithTwoAchievements_ReturnObject()
        {
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new("FileCaseTests\\AchievementsStructure\\twoAchievementsTest.txt");

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(17));
            Assert.That(result.ElementAt(0).Localization, Is.EqualTo("NEW_ACHIEVEMENT_7_2"));
            Assert.That(result.ElementAt(0).Possible?.NormalProvinceValues, Is.EqualTo("yes"));
            Assert.That(result.ElementAt(0).Possible?.NormalOrHistoricalNations, Is.EqualTo("yes"));
            var dlcNames = result.FirstOrDefault()?.Possible?.HasOneOfDlc?.Names;
            Assert.That(dlcNames?.Count, Is.EqualTo(2));
            Assert.That(dlcNames?.FirstOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc 2"));
            Assert.That(dlcNames?.LastOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc 1"));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo(26));
            Assert.That(result.ElementAt(1).Localization, Is.EqualTo("NEW_ACHIEVEMENT_4_8"));
            var dlcNames2 = result.LastOrDefault()?.VisibleRequirements?.HasOneOfDlc?.Names;
            Assert.That(dlcNames2?.Count, Is.EqualTo(2));
            Assert.That(dlcNames2?.FirstOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc4"));
            Assert.That(dlcNames2?.LastOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc2"));
        }
    }
}
