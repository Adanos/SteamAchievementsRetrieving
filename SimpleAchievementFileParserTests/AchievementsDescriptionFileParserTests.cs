using NUnit.Framework;

namespace SimpleAchievementFileParserTests
{
    public class AchievementsDescriptionFileParserTests
    {
        [Test]
        public void AchievementsDescriptionFileParser_ParseSimpleFile_ReturnObject()
        {
            var path = Path.Combine("FileCaseTests", "AchievementsDescription", "description01.txt");
            SimpleAchievementFileParser.AchievementsDescriptionFileParser simpleParser = new(path);

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Value.Id, Is.EqualTo("NEW_ACHIEVEMENT_1"));
            Assert.That(result.FirstOrDefault().Value.Name, Is.EqualTo("Test name"));
            Assert.That(result.FirstOrDefault().Value.Description, Is.EqualTo("Test description"));
        }

        [Test]
        public void AchievementsDescriptionFileParser_ParseFileWithTwoDescription_ReturnObject()
        {
            var path = Path.Combine("FileCaseTests", "AchievementsDescription", "description02.txt");
            SimpleAchievementFileParser.AchievementsDescriptionFileParser simpleParser = new(path);

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(0).Value.Id, Is.EqualTo("NEW_ACHIEVEMENT_1"));
            Assert.That(result.ElementAt(0).Value.Name, Is.EqualTo("Test name"));
            Assert.That(result.ElementAt(0).Value.Description, Is.EqualTo("Test description"));
            Assert.That(result.ElementAt(1).Value.Id, Is.EqualTo("NEW_ACHIEVEMENT_2"));
            Assert.That(result.ElementAt(1).Value.Name, Is.EqualTo("Second test name"));
            Assert.That(result.ElementAt(1).Value.Description, Is.EqualTo("Second test description"));
        }

        [Test]
        public void AchievementsDescriptionFileParser_ParseFileWithMissingDescriptions_ReturnObject()
        {
            var path = Path.Combine("FileCaseTests", "AchievementsDescription", "description03.txt");
            SimpleAchievementFileParser.AchievementsDescriptionFileParser simpleParser = new(path);

            var result = simpleParser.ParseFile();
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.ElementAt(0).Value.Id, Is.EqualTo("NEW_ACHIEVEMENT_1"));
            Assert.That(result.ElementAt(0).Value.Name, Is.EqualTo("Test name"));
            Assert.That(result.ElementAt(0).Value.Description, Is.Null);
            Assert.That(result.ElementAt(1).Value.Id, Is.EqualTo("NEW_ACHIEVEMENT_2"));
            Assert.That(result.ElementAt(1).Value.Name, Is.EqualTo("Second test name"));
            Assert.That(result.ElementAt(1).Value.Description, Is.EqualTo("Second test description"));
            Assert.That(result.ElementAt(2).Value.Id, Is.EqualTo("NEW_ACHIEVEMENT_3"));
            Assert.That(result.ElementAt(2).Value.Name, Is.Null);
            Assert.That(result.ElementAt(2).Value.Description, Is.EqualTo("Third test description"));
        }
    }
}
