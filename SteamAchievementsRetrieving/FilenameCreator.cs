using SteamAchievementsRetrieving.Models;

namespace SteamAchievementsRetrieving
{
    class FilenameCreator
    {
        private readonly SteamAchievementConfiguration SteamAchievementConfiguration;
        private readonly PlayerStats PlayerStats;

        public FilenameCreator(SteamAchievementConfiguration steamAchievementConfiguration, PlayerStats playerStats)
        {
            SteamAchievementConfiguration = steamAchievementConfiguration;
            PlayerStats = playerStats;
        }

        public string CreateFilename(string extension = Constants.CsvExtension)
        {
            string achievements = Constants.Achievements;
            if (SteamAchievementConfiguration.IsAchieved == true)
                achievements = Constants.UnlockedAchievements;
            else if (SteamAchievementConfiguration.IsAchieved == false)
                achievements = Constants.LockedAchievements;
            return SteamAchievementConfiguration.FilePathToSaveResult + achievements + PlayerStats.GameName + extension;
        }
    }
}
