namespace SimpleAchievementFileParser.Model
{
    public class NotModel : INodeAddAble
    {
        public List<KeyValuePair<string, string>> Names { get; set; } = [];
        public List<INodeAddAble?> Nodes { get; set; } = [];
        public string NumOfCustomNations { get; set; } //num_of_custom_nations
        
        private readonly INodeAddAble? Parent;

        public NotModel(INodeAddAble? parent)
        {
            Parent = parent;
        }

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void Add(string token, string value)
        {
            if (token == "num_of_custom_nations")
                NumOfCustomNations = value;
            else Names.Add(new KeyValuePair<string, string>(token, value));
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
            Nodes.Add(node);
        }
    }
}
