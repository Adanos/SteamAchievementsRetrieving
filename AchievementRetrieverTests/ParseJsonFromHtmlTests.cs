using System.IO;
using System.Linq;
using NUnit.Framework;
using AchievementRetriever;
using AchievementRetriever.JsonParsers;
using AchievementRetriever.Models;

namespace AchievementRetrieverTests;

public class ParseJsonFromHtmlTests
{
    [Test]
    public void ParseJsonFromHtmlTests_ParseFileWithTwoDescription_ReturnObject()
    {
        var path = Path.Combine("HtmlTestCase", "GogAchievementsTestCase.txt");
        var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        string jsonFromHtml = reader.ReadToEnd();
        AchievementSourceConfiguration achievementSourceConfiguration = new()
        {
            Name = AchievementSource.GoG
        };
        AchievementParserDispatcher achievementParserDispatcher = new AchievementParserDispatcher(achievementSourceConfiguration);
        ParseJsonFromHtml simpleParser = new(achievementParserDispatcher);

        var result = simpleParser.ParseHtml(jsonFromHtml);
        Assert.That(result.Achievements.Count, Is.EqualTo(2));
        Assert.That(result.Achievements.First().Name, Is.EqualTo("Doge Coins"));
        Assert.That(result.Achievements.First().Description, Is.EqualTo("Starting as Venice, become the best."));
        Assert.That(result.Achievements.First().IsUnlocked, Is.True);
        Assert.That(result.Achievements.Last().Name, Is.EqualTo("New achievement"));
        Assert.That(result.Achievements.Last().Description, Is.EqualTo("Starting as any Mayan country, conquer the world"));
        Assert.That(result.Achievements.Last().IsUnlocked, Is.False);
    }
}