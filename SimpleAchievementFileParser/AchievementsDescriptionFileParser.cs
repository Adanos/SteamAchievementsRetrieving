﻿using SimpleAchievementFileParser.Model;

namespace SimpleAchievementFileParser
{
    public class AchievementsDescriptionFileParser
    {
        private readonly string _fileName;

        public AchievementsDescriptionFileParser(string fileName)
        {
            _fileName = fileName;
        }

        public IDictionary<string, AchievementDescription> ParseFile()
        {
            string line;
            IDictionary<string, AchievementDescription> achievementsDescriptions = new Dictionary<string, AchievementDescription>();
            var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            AchievementDescription? currentAchievementDescription = null;
            string[] delimiters = ["_NAME:0 ", "_DESC:0 ", "_NAME:1 ", "_DESC:1 "];

            while ((line = reader.ReadLine()) != null)
            {
                var splitedLine = line.Split(delimiters, StringSplitOptions.None);

                if (splitedLine[0].Contains(Constants.NewAchievement))
                {
                    if (!achievementsDescriptions.ContainsKey(splitedLine[0]))
                    {
                        currentAchievementDescription = new AchievementDescription(splitedLine[0]);

                        achievementsDescriptions.Add(currentAchievementDescription.Id, currentAchievementDescription);
                    }
   
                    if (line.Contains(Constants.AchievementName))
                    {
                        currentAchievementDescription?.AddName(splitedLine[1].Replace("\"", ""));
                    }
                    else if (line.Contains(Constants.AchievementDescription))
                    {
                        currentAchievementDescription?.AddDescription(splitedLine[1].Replace("\"", ""));
                    }
                }
            }

            return achievementsDescriptions;
        }
    }
}