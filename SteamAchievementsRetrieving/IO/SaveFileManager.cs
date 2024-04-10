using SteamAchievementsRetrieving.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SteamAchievementsRetrieving.IO
{
    public class SaveFileManager
    {
        public string Filename { get; }
        private IList<Achievement> Achievements { get; }
        public ISet<string> DlcNames { get; }

        public SaveFileManager(string filename, ISet<string> _dlcNames, IList<Achievement> achievements)
        {
            Filename = filename;
            Achievements = achievements;
            DlcNames = _dlcNames;
        }

        private string CreateCsvFile()
        {
            StringBuilder stringBuilder = new StringBuilder(Constants.HeaderOfCsvFile + string.Join(";", DlcNames) + Environment.NewLine);
            foreach (var achievement in Achievements ?? Enumerable.Empty<Achievement>())
                stringBuilder.Append($"{achievement.Name}{Constants.Separator}{achievement.Description}{Constants.Separator}{achievement.Country}{Constants.Separator}" +
                    $"{achievement.IsRequiredDlc}{Constants.Separator}{string.Join(Constants.Separator, achievement?.AllRequiredDlcNames ?? [])}{Environment.NewLine}");

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
