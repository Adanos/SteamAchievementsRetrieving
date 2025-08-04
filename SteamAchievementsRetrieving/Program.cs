using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SteamAchievementsRetrieving.Managers;
using System.Net.Http;
using System.Threading.Tasks;
using SteamAchievementsRetrieving.JsonParsers;
using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Models.FromApi.Gog;
using SteamAchievementsRetrieving.Models.FromApi.Steam;

namespace SteamAchievementsRetrieving
{
    class Program
    {
        private static IConfiguration Configuration;

        static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            var achievementManager = services.GetRequiredService<AchievementManager>();
            
            await achievementManager.CreateAchievements();
            
            if (achievementManager.Achievements != null)
            {
                AchievementMatrixCreator achievementMatrixCreator = new AchievementMatrixCreator(achievementManager.Achievements);
                achievementMatrixCreator.CreateMatrix();
                var updatedAchievements = achievementManager.Achievements;
                achievementManager.SaveAchievementsToFile("updated", updatedAchievements);
            }     
        }
        
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<HttpClient>();
                    services.AddSingleton<AchievementManager>();
                    services.AddSingleton<SteamAchievementParser>();
                    services.AddSingleton<GogAchievementParser>();
                    services.AddSingleton<AchievementSourceConfiguration>();  
                    services.AddSingleton<SteamAchievementConfiguration>(); 
                    services.AddSingleton<GogAchievementConfiguration>();   
                    services.AddSingleton<IAchievementParserDispatcher, AchievementParserDispatcher>();
                });
    }
}
