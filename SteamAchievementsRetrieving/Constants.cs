namespace SteamAchievementsRetrieving
{
    public static class Constants
    {
        public const string HeaderJsonType = "application/json",
            ApplicationIdWithQuestionMarkParam = "?appid",
            AuthentificationKeyParam = "key",
            SteamIdParam = "steamid",
            LanguageParam = "l",
            HeaderOfCsvFile = "Achievement;Description;Country;Is dlc required?;",
            Separator = ";",
            CsvExtension = ".csv",
            Achievements = "Achievements ",
            UnlockedAchievements = "UnlockedAchievements ",
            LockedAchievements = "LockedAchievements ",
            And = "AND",
            Or = "OR";
    }
}
