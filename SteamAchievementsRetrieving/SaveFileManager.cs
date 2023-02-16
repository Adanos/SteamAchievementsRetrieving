using SteamAchievementsRetrieving.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SteamAchievementsRetrieving
{
    public class SaveFileManager
    {
        public string Filename { get; }
        public IList<Achievement> Achievements { get; }

        public SaveFileManager(string filename, IList<Achievement> achievements)
        {
            Filename = filename;
            Achievements = achievements;
        }

        private string CreateCsvFile()
        {
            string separator = ";";
            StringBuilder stringBuilder = new StringBuilder(Constants.HeaderOfCsvFile + Environment.NewLine);
            foreach (var achievement in Achievements ?? Enumerable.Empty<Achievement>())
                stringBuilder.Append(string.Format("{0}{1}{2}{3}", achievement.Name, separator, achievement.Description, Environment.NewLine));

            return stringBuilder.ToString();
        }

        public void SaveCsvFile()
        {
            string createText = CreateCsvFile();
            Directory.CreateDirectory(Path.GetDirectoryName(Filename));
            using StreamWriter writer = new StreamWriter(Filename);
            writer.Write(createText);
        }
    }
}
