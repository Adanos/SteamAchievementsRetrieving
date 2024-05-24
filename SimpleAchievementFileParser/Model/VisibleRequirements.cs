namespace SimpleAchievementFileParser.Model
{
    public class VisibleRequirements(INodeAddAble? parent) : INodeAddAble
    {
        public IList<string> HasAllDlc { get; set; } = [];

        public List<OrModel> HasOneOfDlc { get; set; } = []; //OR
        private readonly INodeAddAble? Parent = parent;

        public void Add(string token, string value)
        {
            if (token == Constants.TokenHasDlc)
                HasAllDlc.Add(value);
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
            if (node is OrModel)
                HasOneOfDlc.Add(node as OrModel);
        }

        public INodeAddAble? GetParent()
        {
            return Parent;
        }
    }
}
