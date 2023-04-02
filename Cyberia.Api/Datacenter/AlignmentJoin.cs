namespace Cyberia.Api.Datacenter
{
    [Table("AlignmentsJoin")]
    public sealed class AlignmentJoin : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int AlignmentId { get; set; }

        [NotNull]
        public int TargetAlignmentId { get; set; }

        [NotNull]
        public bool CanJoin { get; set; }

        public AlignmentJoin()
        {
            Id = string.Empty;
        }
    }
}
