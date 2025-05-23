using NUnit.Framework;
using SimpleAchievementFileParser.Model;

namespace SimpleAchievementFileParserTests
{
    class AchievementsStructureFileParserTests
    {
        private Dictionary<string, AchievementDescription> _descriptions = default!;

        [SetUp]
        public void Setup()
        {
            _descriptions = new Dictionary<string, AchievementDescription>()
            {
                { "NEW_ACHIEVEMENT_1_2", new AchievementDescription("15") { Name = "test name 1" } },
                { "NEW_ACHIEVEMENT_7_2", new AchievementDescription("17") { Name = "test name 5" } },
                { "NEW_ACHIEVEMENT_4_8", new AchievementDescription("26") { Name = "test name 4" } },
                { "NEW_ACHIEVEMENT_0_1", new AchievementDescription("26") { Name = "test name 01" } },
                { "NEW_ACHIEVEMENT_11", new AchievementDescription("796") { Name = "test name 7" } }
            };
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithOnlyCommentary_ReturnObject()
        {
            var path = Path.Combine("FileCaseTests", "AchievementsStructure", "achievementOnlyCommentaryTest.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(path);

            var result = simpleParser.ParseFile(_descriptions);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseSimpleFile_ReturnObject()
        {
            var path = Path.Combine("FileCaseTests", "AchievementsStructure", "achievementTest.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(path);

            var result = simpleParser.ParseFile(_descriptions);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(15));
            Assert.That(result.FirstOrDefault()?.Name, Is.EqualTo("test name 1"));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFile_ReturnObject()
        {
            var path = Path.Combine("FileCaseTests", "AchievementsStructure", "achievementTest2.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(path);

            var result = simpleParser.ParseFile(_descriptions);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(17));
            Assert.That(result.FirstOrDefault()?.Name, Is.EqualTo("test name 5"));
            Assert.That(result.FirstOrDefault()?.Possible?.NormalProvinceValues, Is.EqualTo("yes"));
            Assert.That(result.FirstOrDefault()?.Possible?.NormalOrHistoricalNations, Is.EqualTo("yes"));
        }

        [Test]
        public void AchievementsStructureFileParser_CreateAchievements_ReturnObject()
        {
            var path = Path.Combine("QueueCaseTests", "QueueCaseTest2.txt");
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            string? line = string.Empty;

            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                queue.Enqueue(new KeyValuePair<string, string>(values[0], values[1]));
            }

            var testCasesPath = Path.Combine("FileCaseTests", "AchievementsStructure", "achievementTest2.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(testCasesPath);

            var result = simpleParser.CreateAchievements(queue);
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(17));
            Assert.That(result.FirstOrDefault()?.Name, Is.EqualTo("test name 5"));
            Assert.That(result.FirstOrDefault()?.Possible?.NotModel, Is.Not.Null);
            Assert.That(result.FirstOrDefault()?.Possible?.HasOneOfDlc, Is.Not.Null);
            Assert.That(result.FirstOrDefault()?.Possible?.HasOneOfDlc?.Names.Count, Is.EqualTo(2));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithTwoOrsInVisible_ReturnObject()
        {
            var testCasesPath = Path.Combine("FileCaseTests", "AchievementsStructure", "achievementTwoOrsInVisibleTest.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(testCasesPath);

            var result = simpleParser.ParseFile(_descriptions);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(26));
            Assert.That(result.FirstOrDefault()?.Name, Is.EqualTo("test name 4"));
            Assert.That(result.FirstOrDefault()?.VisibleRequirements?.HasOneOfDlc, Is.Not.Null);
            var dlcNames = result.FirstOrDefault()?.VisibleRequirements?.HasOneOfDlc?.FirstOrDefault()?.Names;
            Assert.That(dlcNames?.Count, Is.EqualTo(2));
            Assert.That(dlcNames?.FirstOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc4"));
            Assert.That(dlcNames?.LastOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc2"));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithTokenProvinceId_ReturnObject()
        {
            var testCasesPath = Path.Combine("FileCaseTests", "AchievementsStructure", "achievementWithProvinceIdTest.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(testCasesPath);

            var result = simpleParser.ParseFile(_descriptions);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.Id, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault()?.Name, Is.EqualTo("test name 01"));
            Assert.That(result.FirstOrDefault()?.UnspecifiedNodes?.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault()?.UnspecifiedNodes?.FirstOrDefault()?.NodeName, Is.EqualTo("provinces_to_highlight"));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithTwoAchievements_ReturnObject()
        {
            var testCasesPath = Path.Combine("FileCaseTests", "AchievementsStructure", "twoAchievementsTest.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(testCasesPath);

            var result = simpleParser.ParseFile(_descriptions);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(17));
            Assert.That(result.ElementAt(0).Name, Is.EqualTo("test name 5"));
            Assert.That(result.ElementAt(0).Possible?.NormalProvinceValues, Is.EqualTo("yes"));
            Assert.That(result.ElementAt(0).Possible?.NormalOrHistoricalNations, Is.EqualTo("yes"));
            var dlcNames = result.FirstOrDefault()?.Possible?.HasOneOfDlc?.Names;
            Assert.That(dlcNames?.Count, Is.EqualTo(2));
            Assert.That(dlcNames?.FirstOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc 2"));
            Assert.That(dlcNames?.LastOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc 1"));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo(26));
            Assert.That(result.ElementAt(1).Name, Is.EqualTo("test name 4"));
            var dlcNames2 = result.LastOrDefault()?.VisibleRequirements?.HasOneOfDlc?.FirstOrDefault()?.Names;
            Assert.That(dlcNames2?.Count, Is.EqualTo(2));
            Assert.That(dlcNames2?.FirstOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc4"));
            Assert.That(dlcNames2?.LastOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc2"));
        }

        [Test]
        public void AchievementsStructureFileParser_ParseFileWithManyOrTags_ReturnObject()
        {
            var testCasesPath = Path.Combine("FileCaseTests", "AchievementsStructure", "achievementWithManyOrTags.txt");
            SimpleAchievementFileParser.AchievementsStructureFileParser simpleParser = new(testCasesPath);

            var result = simpleParser.ParseFile(_descriptions);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(796));
            Assert.That(result.ElementAt(0).Name, Is.EqualTo("test name 7"));
            Assert.That(result.ElementAt(0).Possible?.NormalProvinceValues, Is.EqualTo("yes"));
            Assert.That(result.ElementAt(0).Possible?.NormalOrHistoricalNations, Is.EqualTo("yes"));
            var dlcNames = result.FirstOrDefault()?.VisibleRequirements?.HasOneOfDlc;
            Assert.That(dlcNames?.Count, Is.EqualTo(2));
            Assert.That(dlcNames?.SelectMany(x => x.Names)?.FirstOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc5"));
            Assert.That(dlcNames?.SelectMany(x => x.Names)?.LastOrDefault(x => x.Key == "has_dlc").Value, Is.EqualTo("Dlc6"));
        }
    }
}
