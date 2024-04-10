using Microsoft.Extensions.Configuration;
using SteamAchievementsRetrieving.Managers;

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
            achievementManager.CreateAchievements();

            if (achievementManager.Achievements != null)
            {
                AchievementMatrixCreator achievementMatrixCreator = new AchievementMatrixCreator(achievementManager.Achievements);
                achievementMatrixCreator.CreateMatrix();
                var updatedAchievements = achievementManager.Achievements;
                achievementManager.SaveAchievementsToFile("updated", updatedAchievements);
            }     
        }
    }
}
