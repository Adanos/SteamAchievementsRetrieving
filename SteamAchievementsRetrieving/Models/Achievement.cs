using SteamAchievementsRetrieving.Models.FromApi;
using System.Collections.Generic;

namespace SteamAchievementsRetrieving.Models
{
    public class Achievement : AchievementResponse
    {
        public string Country { get; set; }
        public bool IsRequiredDlc { get; set; }
        public IList<string> AllRequiredDlcNames { get; set; } = [];
        public IList<string> OneRequiredOfDlcNames { get; set; } = [];
    }
}
