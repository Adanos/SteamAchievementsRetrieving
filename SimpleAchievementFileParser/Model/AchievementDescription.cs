namespace SimpleAchievementFileParser.Model
{
    public class AchievementDescription(string id) : IEquatable<AchievementDescription>
    {
        public string Id { get; } = id;
        public string? Name { get; set; }
        public string? Description { get; set; }

        public bool Equals(AchievementDescription? other) => (other?.Id, other?.Name, other?.Description).Equals((Id, Name, Description));

        public override int GetHashCode() => (Id, Name, Description).GetHashCode();

        internal void AddName(string name)
        {
            Name = name;
        }

        internal void AddDescription(string description)
        {
            Description = description;
        }
    }
}
