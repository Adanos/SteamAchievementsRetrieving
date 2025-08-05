using System;
using AchievementRetriever.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AchievementRetriever;

public class AchievementsRetrievingFactory : IAchievementsRetrievingFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AchievementSource _source;

    public AchievementsRetrievingFactory(IConfiguration config, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _source = config.GetSection(nameof(AchievementSourceConfiguration)).Get<AchievementSourceConfiguration>().Name;
    }

    public IAchievementsRetrieving GetAchievementsRetrieving()
    {
        return _source switch
        {
            AchievementSource.Steam => _serviceProvider.GetRequiredService<SteamAchievementsRetrieving>(),
            AchievementSource.GoG => _serviceProvider.GetRequiredService<GogAchievementsRetrieving>(),
            _ => throw new NotImplementedException()
        };
    }
}