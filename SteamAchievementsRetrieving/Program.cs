using Microsoft.Extensions.Configuration;
using SteamAchievementsRetrieving.IO;
using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Services;
using System.Collections.Generic;
using System.IO;

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
            FilenameCreator filenameCreator = new FilenameCreator(steamAchievementConfiguration);
            string pattern = filenameCreator.CreateFilename(@"*");
            string[] files = Directory.GetFiles(steamAchievementConfiguration.FilePathToSaveResult, pattern, SearchOption.TopDirectoryOnly);
            IList<Achievement> achievements;
            if (files.Length > 0)
            {
                ReadFileManager readFileManager = new ReadFileManager();
                achievements = readFileManager.ReadAchievementsFromFile(files[0]);
            }
            else
            {
                SteamAchievementsRetrieving steamAchievementsRetrieving = new SteamAchievementsRetrieving(steamAchievementConfiguration);
                var results = steamAchievementsRetrieving.GetAllAchievements().PlayerStats;
                achievements = results.Achievements;
                AchievementManager achievementManager = new AchievementManager(achievements);

                if (steamAchievementConfiguration.IsAchieved == true)
                    achievements = achievementManager.GetUnlockedAchievements();
                else if (steamAchievementConfiguration.IsAchieved == false)
                    achievements = achievementManager.GetLockedAchievements();

                SaveFileManager saveFileManager = new SaveFileManager(filenameCreator.CreateFullPath(results.GameName), achievements);
                saveFileManager.SaveCsvFile();
            }

            AchievementMatrixCreator achievementMatrixCreator = new AchievementMatrixCreator(achievements);
            achievementMatrixCreator.CreateMatrix();
        }
    }
}
