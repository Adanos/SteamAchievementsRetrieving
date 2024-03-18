namespace SimpleAchievementFileParser.Model
{
    public class Achievement : INodeAddAble
    {
        public int Id { get; set; }
        public string Localization { get; set; }
        public Possible? Possible { get; set; }
        public Happened? Happened { get; set; }
        public VisibleRequirements? VisibleRequirements { get; set; }

        public void Add(string token, string value)
        {
            if (token == "id")
                Id = int.Parse(value);
            else if (token == "localization")
                Localization = value;
        }

        void INodeAddAble.Add(INodeAddAble node)
        {
            if (node is Possible)
                Possible = node as Possible;
            else if (node is Happened)
                Happened = node as Happened;
            else if (node is VisibleRequirements)
                VisibleRequirements = node as VisibleRequirements;
        }
    }
}
