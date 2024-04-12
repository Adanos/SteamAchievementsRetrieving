namespace SimpleAchievementFileParser.Model
{
    public class NotModel : INodeAddAble
    {
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
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
        }
    }
}
