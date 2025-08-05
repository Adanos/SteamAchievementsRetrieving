using System;
using System.Text.Json;
using NUnit.Framework;
using AchievementRetriever.JsonParsers;
using AchievementRetriever.Models;

namespace AchievementRetrieverTests.JsonParsers;

public class AchievementParserDispatcherTests
{
    [TestCase(AchievementSource.Steam)]
    [TestCase(AchievementSource.GoG)]
    public void UseAchievementParser_WhenValidSourceProvided_ShouldCreateWithoutErrors(AchievementSource source)
    {
        var configuration = new AchievementSourceConfiguration { Name = source};
        var dispatcher = new AchievementParserDispatcher(configuration);

        Assert.That(dispatcher, Is.Not.Null);
    }

    [Test]
    public void UseAchievementParser_WhenInvalidSourceProvided_ThrowsInvalidOperationException()
    {
        var configuration = new AchievementSourceConfiguration { Name = (AchievementSource)999 };

        var ex = Assert.Throws<InvalidOperationException>(() => new AchievementParserDispatcher(configuration));
        Assert.That(ex.Message, Does.Contain("Invalid parser"));
    }

    [Test]
    public void UseAchievementParser_WhenInvalidJsonProvided_ThrowsJsonException()
    {
        var configuration = new AchievementSourceConfiguration { Name = AchievementSource.Steam };
        var dispatcher = new AchievementParserDispatcher(configuration);
        var invalidJson = "{Invalid JSON}";

        var ex = Assert.Catch(() => dispatcher.Parse(invalidJson));
        Assert.That(ex, Is.InstanceOf<JsonException>());
    }
}