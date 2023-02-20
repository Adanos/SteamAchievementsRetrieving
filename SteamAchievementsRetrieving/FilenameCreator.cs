using SteamAchievementsRetrieving.Models;

namespace SteamAchievementsRetrieving
{
    class FilenameCreator
    {
        private readonly SteamAchievementConfiguration SteamAchievementConfiguration;

        public FilenameCreator(SteamAchievementConfiguration steamAchievementConfiguration)
        {
            SteamAchievementConfiguration = steamAchievementConfiguration;
        }

        public string CreateFilename(string gameName, string extension = Constants.CsvExtension)
        {
            string achievements = Constants.Achievements;
            if (SteamAchievementConfiguration.IsAchieved == true)
                achievements = Constants.UnlockedAchievements;
            else if (SteamAchievementConfiguration.IsAchieved == false)
                achievements = Constants.LockedAchievements;
            return achievements + gameName + extension;
        }

        public string CreateFullPath(string gameName, string extension = Constants.CsvExtension)
        {
            return SteamAchievementConfiguration.FilePathToSaveResult + CreateFilename(gameName, extension);
        }
    }
}
