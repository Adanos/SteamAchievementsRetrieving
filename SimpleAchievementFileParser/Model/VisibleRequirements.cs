namespace SimpleAchievementFileParser.Model
{
    public class VisibleRequirements(INodeAddAble? parent) : INodeAddAble
    {
        public IList<string> HasAllDlc { get; set; } = [];

        public OrModel? HasOneOfDlc { get; set; } //OR
        private readonly INodeAddAble? Parent = parent;

        public void Add(string token, string value)
        {
            if (token == Constants.TokenHasDlc)
                HasAllDlc.Add(value);
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
            if (node is OrModel && HasOneOfDlc == null)
                HasOneOfDlc = node as OrModel;
        }

        public INodeAddAble? GetParent()
        {
            return Parent;
        }
    }
}
