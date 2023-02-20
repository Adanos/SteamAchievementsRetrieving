using Microsoft.Extensions.Configuration;
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
            IList<Achievement> achievements = new List<Achievement>();
            if (files.Length > 0)
            {
                string line;
                var fileStream = new FileStream(files[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fileStream);
                line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(Constants.Separator);
                    achievements.Add(new Achievement() { Name = words[0], Description = words[1] });
                }
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
