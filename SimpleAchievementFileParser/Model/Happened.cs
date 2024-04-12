namespace SimpleAchievementFileParser.Model
{
    public class Happened(INodeAddAble? parent) : INodeAddAble
    {
        public CustomTriggerTooltip? CustomTriggerTooltip { get; set; } //custom_trigger_tooltip
        private readonly INodeAddAble? Parent = parent;

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void Add(string token, string value)
        {
        }

        void INodeAddAble.Add(INodeAddAble? node)
        {
            CustomTriggerTooltip = node as CustomTriggerTooltip;
        }
    }
}
