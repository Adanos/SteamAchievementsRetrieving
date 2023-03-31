using Microsoft.Extensions.Configuration;
using SteamAchievementsRetrieving.Managers;
using SteamAchievementsRetrieving.Models;
using System.Collections.Generic;

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

            AchievementManager achievementManager = new AchievementManager(Configuration);
            IList<Achievement> achievements = achievementManager.GetAchievements();

            AchievementMatrixCreator achievementMatrixCreator = new AchievementMatrixCreator(achievements);
            achievementMatrixCreator.CreateMatrix();
        }
    }
}
