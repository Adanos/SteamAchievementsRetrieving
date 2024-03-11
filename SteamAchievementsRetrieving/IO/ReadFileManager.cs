using SteamAchievementsRetrieving.Models.FromApi;
using System.Collections.Generic;
using System.IO;

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
                achievements.Add(new Achievement() { Name = words[0], Description = words[1] });
            }

            return achievements;
        }
    }
}
