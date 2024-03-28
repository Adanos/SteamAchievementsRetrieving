namespace SimpleAchievementFileParser.Model
{
    public class UnspecifiedNode(string nodeName, INodeAddAble? parent) : INodeAddAble
    {
        public string NodeName { get; private set; } = nodeName;
        public IDictionary<string, string> Attributes = new Dictionary<string, string>();
        public IList<INodeAddAble> UnspecifiedNodes { get; private set; } = [];
        private INodeAddAble? Parent = parent;

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void Add(string token, string value)
        {
            Attributes.Add(token, value);
        }

        public void Add(INodeAddAble? node)
        {
            if (node != null)
                UnspecifiedNodes.Add(node);
        }

        public void SetParent(INodeAddAble node)
        {
            Parent = node;
        }
    }
}
