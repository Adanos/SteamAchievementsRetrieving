using System;
using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using AchievementRetriever.JsonParsers;

namespace AchievementRetrieverTests.JsonParsers;

public class GogAchievementParserTests
{
    [Test]
    public void CanParse_WhenValidJsonArrayProvided_ReturnsTrue()
    {
        var json = "[{\"achievement\": {\"name\": \"Achievement 1\", \"description\": \"Description 1\"}}]";
        var root = JsonDocument.Parse(json).RootElement;

        var parser = new GogAchievementParser();
        var result = parser.CanParse(root);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CanParse_WhenInvalidJsonArrayProvided_ReturnsFalse()
    {
        var json = "[{\"invalidProperty\": {\"name\": \"Achievement 1\", \"description\": \"Description 1\"}}]";
        var root = JsonDocument.Parse(json).RootElement;

        var parser = new GogAchievementParser();
        var result = parser.CanParse(root);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Parse_WhenValidJsonArrayProvided_ReturnsAchievements()
    {
        var json = "window.profilesData.achievements=[{\"achievement\": {\"name\": \"Achievement 1\", \"description\": \"Description 1\"}, \"stats\":{\"11\":{\"isUnlocked\": true}}}];";

        var parser = new GogAchievementParser();
        var result = parser.Parse(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Achievement 1"));
        Assert.That(result[0].Description, Is.EqualTo("Description 1"));
        Assert.That(result[0].IsUnlocked, Is.True);
    }

    [Test]
    public void Parse_WhenStatsAreMissing_ReturnsAchievementsWithNullUnlocked()
    {
        var json = "window.profilesData.achievements=[{\"achievement\":{\"name\":\"Achievement 1\",\"description\":\"Description 1\",\"isUnlocked\":false}}];";

        var parser = new GogAchievementParser();
        var result = parser.Parse(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Achievement 1"));
        Assert.That(result[0].Description, Is.EqualTo("Description 1"));
        Assert.That(result[0].IsUnlocked, Is.Null);
    }

    [Test]
    public void Parse_WhenAchievementPropertyIsMissing_ThrowsKeyNotFoundException()
    {
        var json = "window.profilesData.achievements=[{\"invalidProperty\":{\"name\":\"name\",\"description\":\"desc\",\"isUnlocked\":false}}];";

        var parser = new GogAchievementParser();

        Assert.Throws<KeyNotFoundException>(() => parser.Parse(json));
    }
    
    [Test]
    public void Parse_WhenAchievementPropertyIsMissing_ThrowsNotFoundException()
    {
        var json = "[{\"achievement\": {\"name\": \"Achievement 1\", \"description\": \"Description 1\"}}]";

        var parser = new GogAchievementParser();

        Assert.Throws<Exception>(() => parser.Parse(json));
    }
}