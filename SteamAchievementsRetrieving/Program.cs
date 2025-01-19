using Microsoft.Extensions.Configuration;
using SteamAchievementsRetrieving.Managers;
using System.Threading.Tasks;

namespace SteamAchievementsRetrieving
{
    class Program
    {
        private static IConfiguration Configuration;

        static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            AchievementManager achievementManager = new AchievementManager(Configuration);
            await achievementManager.CreateAchievements();

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
