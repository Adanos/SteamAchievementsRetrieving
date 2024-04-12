namespace SimpleAchievementFileParser.Model
{
    public class OrModel(INodeAddAble? parent) : INodeAddAble
    {
        public List<KeyValuePair<string, string>> Names { get; set; } = [];
        private readonly INodeAddAble? Parent = parent;

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void Add(string token, string value)
        {
            Names.Add(new KeyValuePair<string, string>(token, value));
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
        }
    }
}
