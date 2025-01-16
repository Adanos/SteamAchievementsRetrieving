namespace SimpleAchievementFileParser.Model
{
    public class UnspecifiedNode(string nodeName, INodeAddAble? parent) : INodeAddAble
    {
        public IDictionary<string, string> Attributes = new Dictionary<string, string>();
        public IList<INodeAddAble> UnspecifiedNodes { get; private set; } = [];
        public string NodeName { get; private set; } = nodeName;
        private readonly INodeAddAble? Parent = parent;

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void Add(string token, string value)
        {
            if (!Attributes.ContainsKey(token))
                Attributes.Add(token, value);
            else Attributes[token] += ", " + value;
        }

        public void Add(INodeAddAble? node)
        {
            if (node != null)
                UnspecifiedNodes.Add(node);
        }
    }
}
