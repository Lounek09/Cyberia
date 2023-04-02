namespace Cyberia.Api.Datacenter
{
    [Table("AlignmentSpecializations")]
    public sealed class AlignmentSpecialization : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int AlignmentOrderId { get; set; }

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
