namespace AchievementRetriever.Models.FromApi.Gog
{
    public class GogAchievementConfiguration
    {
        public string AddressApi { get; set; }
        public string User { get; set; }
        public string GameId { get; set; }
        public bool? IsAchieved { get; set; }
        public string FilePathToSaveResult { get; set; }
    }
}