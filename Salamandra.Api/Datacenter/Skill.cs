namespace Salamandra.Api.Datacenter
{
    [Table("Skills")]
    public sealed class Skill
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int JobId { get; set; }

        [NotNull]
        public int InteractiveObjectId { get; set; }

        [NotNull]
        public string Criterion { get; set; }

        [NotNull]
        public int ItemTypeIdForgemagus { get; set; }

        [NotNull]
        public int HaverstedItemId { get; set; }

        public Skill()
        {
            Description = string.Empty;
            Criterion = string.Empty;
        }
    }
}
