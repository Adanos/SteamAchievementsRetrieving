namespace SimpleAchievementFileParser.Model
{
    public class NotModel : INodeAddAble
    {
        public string NumOfCustomNations { get; set; } //num_of_custom_nations

        public void Add(string token, string value)
        {
            if (token == "num_of_custom_nations")
                NumOfCustomNations = value;
        }

        void INodeAddAble.Add(INodeAddAble possible)
        {
            throw new NotImplementedException();
        }
    }
}
