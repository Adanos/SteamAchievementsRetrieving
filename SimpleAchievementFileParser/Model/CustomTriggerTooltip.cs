namespace SimpleAchievementFileParser.Model
{
    public class CustomTriggerTooltip : INodeAddAble
    {
        public string Tooltip { get; set; } //tooltip 
        public string HasCountryFlag { get; set; } //has_country_flag 
        private INodeAddAble? Parent;

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
            if (token == "tooltip")
                Tooltip = value;
            if (token == "has_country_flag")
                HasCountryFlag = value;
        }

        public void SetParent(INodeAddAble node)
        {
            Parent = node;
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
        }
    }
}
