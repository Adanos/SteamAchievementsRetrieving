using Microsoft.Extensions.Configuration;
using SteamAchievementsRetrieving.IO;
using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Services;
using System.Collections.Generic;
using System.IO;

namespace SteamAchievementsRetrieving.Managers
{
    internal class AchievementManager
    {
        private readonly SteamAchievementConfiguration _steamAchievementConfiguration;
        private readonly FilenameCreator _filenameCreator;

        public AchievementManager(IConfiguration configuration)
        {
            _steamAchievementConfiguration = configuration.GetSection(nameof(SteamAchievementConfiguration)).Get<SteamAchievementConfiguration>();
            _filenameCreator = new FilenameCreator(_steamAchievementConfiguration);  
        }

        public IList<Achievement> GetAchievements()
        {
            string pattern = _filenameCreator.CreateFilename(@"*");
            string[] files = Directory.GetFiles(_steamAchievementConfiguration.FilePathToSaveResult, pattern, SearchOption.TopDirectoryOnly);
            IList<Achievement> achievements;

            if (files.Length > 0)
            {
                ReadFileManager readFileManager = new ReadFileManager();
                achievements = readFileManager.ReadAchievementsFromFile(files[0]);
            }
            else
            {
                SteamAchievementsRetrieving steamAchievementsRetrieving = new SteamAchievementsRetrieving(_steamAchievementConfiguration);
                var results = steamAchievementsRetrieving.GetAllAchievements().PlayerStats;
                achievements = results.Achievements;
                AchievementGrouping achievementGrouping = new AchievementGrouping(achievements);

                if (_steamAchievementConfiguration.IsAchieved == true)
                    achievements = achievementGrouping.GetUnlockedAchievements();
                else if (_steamAchievementConfiguration.IsAchieved == false)
                    achievements = achievementGrouping.GetLockedAchievements();

                SaveFileManager saveFileManager = new SaveFileManager(_filenameCreator.CreateFullPath(results.GameName), achievements);
                saveFileManager.SaveCsvFile();
            }

            return achievements;
        }
    }
}
