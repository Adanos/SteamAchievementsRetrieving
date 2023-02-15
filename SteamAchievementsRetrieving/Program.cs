using Microsoft.Extensions.Configuration;
using System;

namespace SteamAchievementsRetrieving
{
    class Program
    {
        private static IConfiguration Configuration;

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            var steamAchievementConfiguration = Configuration.GetSection(nameof(SteamAchievementConfiguration)).Get<SteamAchievementConfiguration>();
            SteamAchievementsRetrieving steamAchievementsRetrieving = new SteamAchievementsRetrieving(steamAchievementConfiguration);
            var results = steamAchievementsRetrieving.GetAchievements();
        }
    }
}
