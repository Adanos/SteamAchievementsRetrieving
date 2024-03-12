namespace SimpleAchievementFileParser.Model
{
    public class Happened : INodeAddAble
    {
        public CustomTriggerTooltip CustomTriggerTooltip { get; set; } //custom_trigger_tooltip
        public void Add(string token, string value = null)
        {
            if (token == "custom_trigger_tooltip")
                CustomTriggerTooltip = new CustomTriggerTooltip();
        }

        void INodeAddAble.Add(INodeAddAble possible)
        {
            CustomTriggerTooltip = possible as CustomTriggerTooltip;
        }
    }
}
