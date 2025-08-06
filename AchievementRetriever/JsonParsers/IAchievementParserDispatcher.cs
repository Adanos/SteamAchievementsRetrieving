namespace AchievementRetriever.JsonParsers;

public interface IAchievementParserDispatcher
{
    IAchievementParser GetParser();
}