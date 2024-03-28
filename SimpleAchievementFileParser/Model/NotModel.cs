namespace SimpleAchievementFileParser.Model
{
    public class NotModel : INodeAddAble
    {
        public string NumOfCustomNations { get; set; } //num_of_custom_nations
        private INodeAddAble? Parent;

        public NotModel(INodeAddAble? parent)
        {
            Parent = parent;
        }

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void SetParent(INodeAddAble node)
        {
            Parent = node;
        }

        public void Add(string token, string value)
        {
            if (token == "num_of_custom_nations")
                NumOfCustomNations = value;
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
        }
    }
}
