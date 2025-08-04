using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SteamAchievementsRetrieving;
using SteamAchievementsRetrieving.JsonParsers;
using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Models.FromApi.Gog;
using SteamAchievementsRetrieving.Models.FromApi.Steam;

namespace SteamAchievementsRetrievingTests;

public class AchievementsRetrievingFactoryTests
{
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddTransient<HttpClient>();
        services.AddTransient<IAchievementParserDispatcher, AchievementParserDispatcher>();
        services.AddTransient<IParseJsonFromHtml, ParseJsonFromHtml>();
        services.AddTransient<AchievementSourceConfiguration>();
        services.AddTransient<GogAchievementConfiguration>();
        services.AddTransient<SteamAchievementConfiguration>();
        services.AddTransient<SteamAchievementsRetrieving.SteamAchievementsRetrieving>();
        services.AddTransient<GogAchievementsRetrieving>();
        _serviceProvider = services.BuildServiceProvider();
    }
    
    [TestCase(AchievementSource.Steam, typeof(SteamAchievementsRetrieving.SteamAchievementsRetrieving))]
    [TestCase(AchievementSource.GoG, typeof(GogAchievementsRetrieving))]
    public void GetAchievementsRetrieving_ReturnsCorrectType_WhenSourceIsValid(AchievementSource source, Type expectedType)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "AchievementSourceConfiguration:Name", source.ToString() }
            })
            .Build();

        var factory = new AchievementsRetrievingFactory(config, _serviceProvider);
        var result = factory.GetAchievementsRetrieving();

        Assert.That(result, Is.InstanceOf(expectedType));
    }

    [Test]
    public void GetAchievementsRetrieving_ThrowsNotImplementedException_WhenSourceIsUnknown()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "AchievementSourceConfiguration:Name", ((AchievementSource)222).ToString() }
            })
            .Build();

        var factory = new AchievementsRetrievingFactory(config, _serviceProvider);

        Assert.Throws<NotImplementedException>(() => factory.GetAchievementsRetrieving());
    }
}