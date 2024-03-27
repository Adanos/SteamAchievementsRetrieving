using Microsoft.Extensions.Configuration;
using SimpleAchievementFileParser;
using SteamAchievementsRetrieving.IO;
using SteamAchievementsRetrieving.Models;
using SteamAchievementsRetrieving.Models.FromApi;
using SteamAchievementsRetrieving.Models.FromGameStructure;
using SteamAchievementsRetrieving.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SteamAchievementsRetrieving.Managers
{
    internal class AchievementManager
    {
        private readonly SteamAchievementConfiguration _steamAchievementConfiguration;
        private readonly EuropaUniversalisFilesStructureConfiguration _europaUniversalisFilesStructureConfiguration;
        private readonly FilenameCreator _filenameCreator;
        private IList<AchievementResponse> AchievementsResponse { get; set; }
        public IList<Achievement> Achievements { get; private set; }

        public AchievementManager(IConfiguration configuration)
        {
            _steamAchievementConfiguration = configuration.GetSection(nameof(SteamAchievementConfiguration)).Get<SteamAchievementConfiguration>();
            _europaUniversalisFilesStructureConfiguration = configuration.GetSection(nameof(EuropaUniversalisFilesStructureConfiguration)).Get<EuropaUniversalisFilesStructureConfiguration>();
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
                    AchievementsResponse = results.PlayerStats.Achievements;
                    FilterAchievements();
                    MapAchievements();
                    SaveAchievementsToFile(results.PlayerStats.GameName);
                }
                else 
                {
                    Console.WriteLine("Error, status code: {0}", results.StatusCode);
                }
            }
        }

        private void MapAchievements()
        {
            string gameDirectory = _europaUniversalisFilesStructureConfiguration.GameDirectory;
            AchievementsDescriptionFileParser achievementsDescriptionFileParser = new(gameDirectory + _europaUniversalisFilesStructureConfiguration.AchievementsLocalisationPath);
            AchievementsStructureFileParser achievementsStructureFileParser = new(gameDirectory + _europaUniversalisFilesStructureConfiguration.AchievementsRequirementsPath);
            var descriptions = achievementsDescriptionFileParser.ParseFile();
            var requirements = achievementsStructureFileParser.ParseFile();

            Achievements = [];
            foreach (var achievement in AchievementsResponse)
            {
                //requirements.FirstOrDefault(x => x.);
                //descriptions.ContainsKey(achievement.Name);
                Achievements.Add(new Achievement()
                {
                    Achieved = achievement.Achieved,
                    Description = achievement.Description,
                    Name = achievement.Name
                });
            }
        }

        private void FilterAchievements()
        {
            AchievementGrouping achievementGrouping = new AchievementGrouping(AchievementsResponse);

            if (_steamAchievementConfiguration.IsAchieved == true)
                AchievementsResponse = achievementGrouping.GetUnlockedAchievements();
            else if (_steamAchievementConfiguration.IsAchieved == false)
                AchievementsResponse = achievementGrouping.GetLockedAchievements();
        }

        private IList<Achievement> ReadAchievementsFromFile(string[] files)
        {
            ReadFileManager readFileManager = new ReadFileManager();
            return readFileManager.ReadAchievementsFromFile(files[0]);
        }

        private void SaveAchievementsToFile(string gameName)
        {
            SaveFileManager saveFileManager = new SaveFileManager(_filenameCreator.CreateFullPath(gameName), Achievements);
            saveFileManager.SaveCsvFile();
        }
    }
}
