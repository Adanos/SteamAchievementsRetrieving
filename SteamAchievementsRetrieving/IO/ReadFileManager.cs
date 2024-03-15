using SteamAchievementsRetrieving.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SteamAchievementsRetrieving.IO
{
    public class ReadFileManager
    {
        public IList<Achievement> ReadAchievementsFromFile(string fileName)
        {
            string line;
            IList<Achievement> achievements = new List<Achievement>();
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            line = reader.ReadLine();
            while ((line = reader.ReadLine()) != null)
            {
                string[] words = line.Split(Constants.Separator);
                bool.TryParse(words[3], out bool isRequiredDlc);
                IList<string> dlcNames = words[4..].Where(x => !string.IsNullOrEmpty(x)).ToList();
                achievements.Add(new Achievement() { Name = words[0], Description = words[1], Country = words[2], IsRequiredDlc = isRequiredDlc, DlcNames = dlcNames });
            }

            return achievements;
        }
    }
}
