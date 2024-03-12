namespace SimpleAchievementFileParser.Model
{
    public class VisibleRequirements : INodeAddAble
    {
        public IList<string> HasAllDlc { get; set; }
        public IList<string> HasOneOfDlc { get; set; }

        public void Add(string token, string value)
        {
            HasAllDlc.Add(value);
        }

        void INodeAddAble.Add(INodeAddAble possible)
        {
            throw new NotImplementedException();
        }
    }
}
