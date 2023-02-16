using Microsoft.Extensions.Configuration;
using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Services;
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

            var steamAchievementConfiguration = Configuration.GetSection(nameof(SteamAchievementConfiguration)).Get<SteamAchievementConfiguration>();
            SteamAchievementsRetrieving steamAchievementsRetrieving = new SteamAchievementsRetrieving(steamAchievementConfiguration);
            var results = steamAchievementsRetrieving.GetAllAchievements().PlayerStats;
            IList<Achievement> achievements = results.Achievements;
            AchievementManager achievementManager = new AchievementManager(achievements);

            if (steamAchievementConfiguration.IsAchieved == true)
                achievements = achievementManager.GetUnlockedAchievements();
            else if (steamAchievementConfiguration.IsAchieved == false) 
                achievements = achievementManager.GetLockedAchievements();

            FilenameCreator filenameCreator = new FilenameCreator(steamAchievementConfiguration, results);
            SaveFileManager saveFileManager = new SaveFileManager(filenameCreator.CreateFilename(), achievements);
            saveFileManager.SaveCsvFile();
        }
    }
}
