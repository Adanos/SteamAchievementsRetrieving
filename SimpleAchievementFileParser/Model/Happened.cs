namespace SimpleAchievementFileParser.Model
{
    public class Happened : INodeAddAble
    {
        public CustomTriggerTooltip? CustomTriggerTooltip { get; set; } //custom_trigger_tooltip
        public void Add(string token, string value)
        {
        }

        void INodeAddAble.Add(INodeAddAble node)
        {
            CustomTriggerTooltip = node as CustomTriggerTooltip;
        }
    }
}
