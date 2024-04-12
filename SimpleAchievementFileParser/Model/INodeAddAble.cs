namespace SimpleAchievementFileParser.Model
{
    public interface INodeAddAble
    {
        void Add(string token, string value);
        void Add(INodeAddAble? node);
        INodeAddAble? GetParent();
    }
}
