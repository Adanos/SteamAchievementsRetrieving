namespace SimpleAchievementFileParser.Model
{
    public class OrModel : INodeAddAble
    {
        public List<KeyValuePair<string, string>> Names { get; set; } = [];

        public void Add(string token, string value)
        {
            Names.Add(new KeyValuePair<string, string>(token, value));
        }

        void INodeAddAble.Add(INodeAddAble node)
        {
            throw new NotImplementedException();
        }
    }
}
