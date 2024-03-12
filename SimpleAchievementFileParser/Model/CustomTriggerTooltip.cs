namespace SimpleAchievementFileParser.Model
{
    public class CustomTriggerTooltip : INodeAddAble
    {
        public string Tooltip { get; set; } //tooltip 
        public string HasCountryFlag { get; set; } //has_country_flag 
        public void Add(string token, string value)
        {
            if (token == "tooltip")
                Tooltip = value;
            if (token == "has_country_flag")
                HasCountryFlag = value;
        }

        void INodeAddAble.Add(INodeAddAble possible)
        {
            throw new NotImplementedException();
        }
    }
}
