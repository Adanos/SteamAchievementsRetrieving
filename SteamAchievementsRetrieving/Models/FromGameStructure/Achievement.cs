namespace SteamAchievementsRetrieving.Models.FromGameStructure
{
    internal class Achievement
    {
        public int Id { get; set; }
        public string Localisation { get; set; }
        public VisibleRequirements VisibleRequirements { get; set; }
    }
}
