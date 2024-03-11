using System.Collections.Generic;

namespace SteamAchievementsRetrieving.Models.FromGameStructure
{
    internal class VisibleRequirements
    {
        public IList<string> HasAllDlc { get; set; }
        public IList<string> HasOneOfDlc { get; set; }
    }
}
