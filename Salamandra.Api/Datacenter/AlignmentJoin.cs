namespace Salamandra.Api.Datacenter
{
    [Table("AlignmentsJoin")]
    public sealed class AlignmentJoin
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int TargetAlignmentId { get; set; }

        [NotNull]
        public bool CanJoin { get; set; }

        public AlignmentJoin()
        {

        }
    }
}
