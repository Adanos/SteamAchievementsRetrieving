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
        public IList<Achievement> Achievements { get; private set; }

        public AchievementManager(IConfiguration configuration)
        {
            _steamAchievementConfiguration = configuration.GetSection(nameof(SteamAchievementConfiguration)).Get<SteamAchievementConfiguration>();
            _filenameCreator = new FilenameCreator(_steamAchievementConfiguration);  
        }

        public void CreateAchievements()
        {
            string pattern = _filenameCreator.CreateFilename(@"*");
            string[] files = Directory.GetFiles(_steamAchievementConfiguration.FilePathToSaveResult, pattern, SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                Achievements = ReadAchievementsFromFile(files);
            }
            else
            {
                SteamAchievementsRetrieving steamAchievementsRetrieving = new SteamAchievementsRetrieving(_steamAchievementConfiguration);
                var results = steamAchievementsRetrieving.GetAllAchievements();

                if (results.Success)
                {
                    Achievements = results.PlayerStats.Achievements;
                    FilterAchievements();

                    SaveAchievementsToFile(results);
                }
            }
        }

        private void FilterAchievements()
        {
            AchievementGrouping achievementGrouping = new AchievementGrouping(Achievements);

            if (_steamAchievementConfiguration.IsAchieved == true)
                Achievements = achievementGrouping.GetUnlockedAchievements();
            else if (_steamAchievementConfiguration.IsAchieved == false)
                Achievements = achievementGrouping.GetLockedAchievements();
        }

        private IList<Achievement> ReadAchievementsFromFile(string[] files)
        {
            ReadFileManager readFileManager = new ReadFileManager();
            return readFileManager.ReadAchievementsFromFile(files[0]);
        }

        private void SaveAchievementsToFile(SteamAchievementResponse results)
        {
            SaveFileManager saveFileManager = new SaveFileManager(_filenameCreator.CreateFullPath(results.PlayerStats.GameName), Achievements);
            saveFileManager.SaveCsvFile();
        }
    }
}
