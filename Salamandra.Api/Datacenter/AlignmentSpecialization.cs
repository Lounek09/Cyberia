namespace Salamandra.Api.Datacenter
{
    [Table("AlignmentSpecializations")]
    public sealed class AlignmentSpecialization
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int AlignmenOrderId { get; set; }

        [NotNull]
        public int AlignmentLevelRequired { get; set; }

        [NotNull]
        public string AlignmentFeatParameters { get; set; }

        public AlignmentSpecialization()
        {
            Name = string.Empty;
            Description = string.Empty;
            AlignmentFeatParameters = string.Empty;
        }
    }
}
