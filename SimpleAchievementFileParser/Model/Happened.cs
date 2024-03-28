namespace SimpleAchievementFileParser.Model
{
    public class Happened : INodeAddAble
    {
        public CustomTriggerTooltip? CustomTriggerTooltip { get; set; } //custom_trigger_tooltip
        private INodeAddAble? Parent;

        public Happened(INodeAddAble? parent)
        {
            Parent = parent;
        }

        public INodeAddAble? GetParent()
        {
            return Parent;
        }

        public void SetParent(INodeAddAble node)
        {
            Parent = node;
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
