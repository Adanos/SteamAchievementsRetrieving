namespace SimpleAchievementFileParser.Model
{
    public class AchievementDescription : IEquatable<AchievementDescription>
    {
        public string Id { get; }
        public string? Name { get; private set; }
        public string? Description { get; private set; }

        public AchievementDescription(string id) 
        {
            Id = id;
        }

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
