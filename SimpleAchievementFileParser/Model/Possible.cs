namespace SimpleAchievementFileParser.Model
{
    public class Possible : INodeAddAble
    {
        public string NormalOrHistoricalNations { get; set; } //normal_or_historical_nations
        public string NormalProvinceValues { get; set; } // normal_province_values
        public NotModel? NotModel { get; set; } // NOT

        public OrModel? HasOneOfDlc { get; set; } //OR

        public IList<UnspecifiedNode>? UnspecifiedNodes { get; private set; }
        private INodeAddAble? Parent;

        public Possible(INodeAddAble? parent)
        {
            Parent = parent;
        }

        public INodeAddAble GetParent()
        {
            return Parent;
        }

        public void SetParent(INodeAddAble node)
        {
            Parent = node;
        }

        public void Add(string token, string value)
        {
            if (token == "normal_or_historical_nations")
                NormalOrHistoricalNations = value;
            if (token == "normal_province_values")
                NormalProvinceValues = value;
        }

        void INodeAddAble.Add(INodeAddAble node)
        {
            if (node is NotModel)
                NotModel = node as NotModel;
            else if (node is OrModel)
                HasOneOfDlc = node as OrModel;
            else if (node is UnspecifiedNode unspecifiedNode)
            {
                UnspecifiedNodes ??= [];
                UnspecifiedNodes.Add(unspecifiedNode);
            }
        }
    }
}
