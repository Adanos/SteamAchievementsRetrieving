using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AchievementRetriever.JsonParsers;
using AchievementRetriever.Models.FromApi;
using Newtonsoft.Json;
using AchievementRetriever.Models;
using AchievementRetriever.Models.FromApi.Gog;

namespace AchievementRetriever;

public class ParseJsonFromHtml(IAchievementParserDispatcher achievementParserDispatcher) : IParseJsonFromHtml
{
    public AchievementsResponse ParseHtml(string jsonFromHtml)
    {
        var match = Regex.Match(jsonFromHtml, @"window\.profilesData\.achievements\s*=\s*(\[.*?\]);", RegexOptions.Singleline);
        if (!match.Success) throw new Exception("Not found achievements.");
        var json = match.Groups[1].Value;
        
        AchievementsResponse result = new AchievementsResponse
        {
            Achievements = achievementParserDispatcher.Parse(json),
            Success = true,
            StatusCode = System.Net.HttpStatusCode.OK
        };
        return result;
    }
}