using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AchievementRetriever.Models;

namespace AchievementRetriever.IO
{
    public class SaveFileManager(string filename, ISet<string> dlcNames, IList<Achievement> achievements)
    {
        public string Filename { get; } = filename;
        private IList<Achievement> Achievements { get; } = achievements;
        public ISet<string> DlcNames { get; } = dlcNames;

        private string CreateCsvFile()
        {
            StringBuilder stringBuilder = new StringBuilder(Constants.HeaderOfCsvFile + string.Join(";", DlcNames) + Environment.NewLine);
            foreach (var achievement in Achievements ?? Enumerable.Empty<Achievement>())
            {
                stringBuilder.Append($"{achievement.Name}{Constants.Separator}{achievement.Description}{Constants.Separator}" +
                    $"{string.Join(Constants.CountriesSeparator, achievement.Countries)}{Constants.Separator}" +
                    $"{achievement.IsRequiredDlc}{Constants.Separator}{MarkDlcWithOperator(achievement)}{Environment.NewLine}");
            }

            return stringBuilder.ToString();
        }

        private string MarkDlcWithOperator(Achievement achievement)
        {
            StringBuilder result = new();
            foreach (var dlcName in DlcNames)
            {
                if (achievement?.AllRequiredDlcNames != null && achievement.AllRequiredDlcNames.Contains(dlcName))
                    result.Append($"AND{Constants.Separator}");
                else if (achievement?.OneRequiredOfDlcNames != null && achievement.OneRequiredOfDlcNames.Contains(dlcName))
                    result.Append($"OR{Constants.Separator}");
                else result.Append(Constants.Separator);
            }
            return result.ToString();
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
