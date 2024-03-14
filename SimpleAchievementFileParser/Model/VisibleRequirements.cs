namespace SimpleAchievementFileParser.Model
{
    public class VisibleRequirements : INodeAddAble
    {
        public IList<string> HasAllDlc { get; set; } = new List<string>();

        public OrModel HasOneOfDlc { get; set; } //OR

        public void Add(string token, string value)
        {
            HasAllDlc.Add(value);
        }

        void INodeAddAble.Add(INodeAddAble node)
        {
            if (node is OrModel && HasOneOfDlc == null)
                HasOneOfDlc = node as OrModel;
        }
    }
}
