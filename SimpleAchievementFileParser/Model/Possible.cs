namespace SimpleAchievementFileParser.Model
{
    public class Possible : INodeAddAble
    {
        public string NormalOrHistoricalNations { get; set; } //normal_or_historical_nations
        public string NormalProvinceValues { get; set; } // normal_province_values
        public NotModel NotModel { get; set; } // NOT

        public OrModel HasOneOfDlc { get; set; } //OR

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
        }
    }
}
