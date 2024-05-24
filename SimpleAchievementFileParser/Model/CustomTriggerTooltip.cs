namespace SimpleAchievementFileParser.Model
{
    public class CustomTriggerTooltip : INodeAddAble
    {
        public string Tooltip { get; set; } //tooltip 
        public string HasCountryFlag { get; set; } //has_country_flag
        public List<INodeAddAble?> Nodes { get; set; } = []; //OR
        public List<KeyValuePair<string, string>> Names { get; set; } = [];
        private readonly INodeAddAble? Parent;

        public CustomTriggerTooltip(INodeAddAble? parent)
        {
            Parent = parent;
        }

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void Add(string token, string value)
        {
            if (token == Constants.TokenTooltip)
                Tooltip = value;
            if (token == Constants.TokenHasCountryFlag)
                HasCountryFlag = value;
            else Names.Add(new KeyValuePair<string, string>(token, value));
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
            Nodes.Add(node);
        }
    }
}
