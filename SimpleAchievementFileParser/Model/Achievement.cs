namespace SimpleAchievementFileParser.Model
{
    public class Achievement : INodeAddAble
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Possible? Possible { get; private set; }
        public Happened? Happened { get; private set; }
        public VisibleRequirements? VisibleRequirements { get; private set; }
        public IList<UnspecifiedNode>? UnspecifiedNodes { get; set; }

        private INodeAddAble? Parent;

        public Achievement(INodeAddAble? parent)
        {
            Parent = parent;
        }
        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void Add(string token, string value)
        {
            if (token == Constants.TokenId)
                Id = int.Parse(value);
            else if (token == Constants.TokenLocalization)
                Name = value;
        }

        public void SetParent(INodeAddAble node)
        {
            Parent = node;
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
            if (node is Possible)
                Possible = node as Possible;
            else if (node is Happened)
                Happened = node as Happened;
            else if (node is VisibleRequirements)
                VisibleRequirements = node as VisibleRequirements;
            else if (node is UnspecifiedNode unspecifiedNode)
            {
                UnspecifiedNodes ??= [];
                UnspecifiedNodes.Add(unspecifiedNode);
            }            
        }
    }
}
