namespace Cyberia.Api.Datacenter
{
    [Table("AlignmentsAttack")]
    public sealed class AlignmentAttack : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int AlignmentId { get; set; }

        [NotNull]
        public int TargetAlignmentId { get; set; }

        [NotNull]
        public bool CanAttack { get; set; }

        public AlignmentAttack()
        {
            Id = string.Empty;
        }
    }
}
