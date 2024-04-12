namespace SimpleAchievementFileParser.Model
{
    public class CustomTriggerTooltip : INodeAddAble
    {
        public string Tooltip { get; set; } //tooltip 
        public string HasCountryFlag { get; set; } //has_country_flag 
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
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
        }
    }
}
