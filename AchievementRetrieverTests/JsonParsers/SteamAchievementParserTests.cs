using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using AchievementRetriever.JsonParsers;

namespace AchievementRetrieverTests.JsonParsers;

public class SteamAchievementParserTests
{
    [Test]
    public void CanParse_WhenValidPlayerStatsProvided_ReturnsTrue()
    {
        var json = "{\"playerstats\": {\"gameName\": \"Game 1\", \"achievements\": []}}";
        var root = JsonDocument.Parse(json).RootElement;

        var parser = new SteamAchievementParser();
        var result = parser.CanParse(root);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CanParse_WhenPlayerStatsMissing_ReturnsFalse()
    {
        var json = "{\"invalidProperty\": {\"gameName\": \"Game 1\", \"achievements\": []}}";
        var root = JsonDocument.Parse(json).RootElement;

        var parser = new SteamAchievementParser();
        var result = parser.CanParse(root);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Parse_WhenValidPlayerStatsProvided_ReturnsAchievements()
    {
        var json = "{\"playerstats\": {\"gameName\": \"Game 1\", \"achievements\": [{\"name\": \"Achievement 1\", \"description\": \"Description 1\", \"achieved\": 1}]}}";

        var parser = new SteamAchievementParser();
        var result = parser.Parse(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].GameName, Is.EqualTo("Game 1"));
        Assert.That(result[0].Name, Is.EqualTo("Achievement 1"));
        Assert.That(result[0].Description, Is.EqualTo("Description 1"));
        Assert.That(result[0].IsUnlocked, Is.True);
    }

    [Test]
    public void Parse_WhenAchievementsArrayIsMissing_ReturnsEmptyList()
    {
        var json = "{\"playerstats\": {\"gameName\": \"Game 1\"}}";

        var parser = new SteamAchievementParser();
        var result = parser.Parse(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_WhenGameNameIsMissing_ThrowsKeyNotFoundException()
    {
        var json = "{\"playerstats\": {\"achievements\": [{\"name\": \"Achievement 1\", \"description\": \"Description 1\", \"achieved\": 1}]}}";

        var parser = new SteamAchievementParser();

        Assert.Throws<KeyNotFoundException>(() => parser.Parse(json));
    }

    [Test]
    public void Parse_WhenAchievementPropertiesAreMissing_ThrowsKeyNotFoundException()
    {
        var json = "{\"playerstats\": {\"gameName\": \"Game 1\", \"achievements\": [{\"invalidProperty\": \"Invalid Value\"}]}}";

        var parser = new SteamAchievementParser();

        Assert.Throws<KeyNotFoundException>(() => parser.Parse(json));
    }
}