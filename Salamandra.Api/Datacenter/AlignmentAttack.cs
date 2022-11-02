namespace Salamandra.Api.Datacenter
{
    [Table("AlignmentsAttack")]
    public sealed class AlignmentAttack
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int TargetAlignmentId { get; set; }

        [NotNull]
        public bool CanAttack { get; set; }

        public AlignmentAttack()
        {

        }
    }
}
