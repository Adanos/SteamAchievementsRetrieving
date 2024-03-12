namespace SimpleAchievementFileParser.Model
{
    public class NotModel : INodeAddAble
    {
        public string Name { get; set; }

        public void Add(string token, string value)
        {
            Name = token;
        }

        void INodeAddAble.Add(INodeAddAble possible)
        {
            throw new NotImplementedException();
        }
    }
}
