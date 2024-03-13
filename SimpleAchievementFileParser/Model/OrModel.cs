namespace SimpleAchievementFileParser.Model
{
    public class OrModel : INodeAddAble
    {
        public List<string> Names { get; set; } = new List<string>();

        public void Add(string token, string value)
        {
            Names.Add(value);
        }

        void INodeAddAble.Add(INodeAddAble possible)
        {
            throw new NotImplementedException();
        }
    }
}
