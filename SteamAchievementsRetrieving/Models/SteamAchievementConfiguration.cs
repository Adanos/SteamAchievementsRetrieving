namespace SteamAchievementsRetrieving.Models
{
    public class SteamAchievementConfiguration
    {
        public string AddressApi { get; set; }
        public string ApplicationId { get; set; }
        public string AuthentificationKey { get; set; }
        public string SteamId { get; set; }
        public string Language { get; set; }
        public bool? IsAchieved { get; set; }
        public string FilePathToSaveResult { get; set; }
    }
}