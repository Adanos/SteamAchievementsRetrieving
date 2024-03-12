namespace SimpleAchievementFileParser.Model
{
    public class Achievement : INodeAddAble
    {
        public int Id { get; set; }
        public string Localization { get; set; }
        public Possible Possible { get; set; }
        public Happened Happened { get; set; }
        public VisibleRequirements VisibleRequirements { get; set; }

        public void Add(string token, string value)
        {
            if (token == "id")
                Id = int.Parse(value);
            else if (token == "localization")
                Localization = value;
        }

        void INodeAddAble.Add(INodeAddAble possible)
        {
            if (possible is Possible)
                Possible = possible as Possible;
            else if (possible is Happened)
                Happened = possible as Happened;
            else if (possible is VisibleRequirements)
                VisibleRequirements = possible as VisibleRequirements;
        }
    }
}
