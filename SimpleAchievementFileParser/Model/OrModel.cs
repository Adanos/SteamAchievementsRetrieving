namespace SimpleAchievementFileParser.Model
{
    public class OrModel : INodeAddAble
    {
        public List<KeyValuePair<string, string>> Names { get; set; } = [];
        private INodeAddAble? Parent;

        public OrModel(INodeAddAble? parent)
        {
            Parent = parent;
        }

        public INodeAddAble GetParent()
        {
            return Parent;
        }

        public void SetParent(INodeAddAble node)
        {
            Parent = node;
        }

        public void Add(string token, string value)
        {
            Names.Add(new KeyValuePair<string, string>(token, value));
        }

        void INodeAddAble.Add(INodeAddAble node)
        {
            //throw new NotImplementedException();
        }
    }
}
