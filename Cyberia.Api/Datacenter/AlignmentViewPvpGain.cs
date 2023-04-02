namespace Cyberia.Api.Datacenter
{
    [Table("AlignmentsViewPvpGain")]
    public sealed class AlignmentViewPvpGain : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int AlignmentId { get; set; }

        [NotNull]
        public int TargetAlignmentId { get; set; }

        [NotNull]
        public bool ViewPvpGain { get; set; }

        public AlignmentViewPvpGain()
        {
            Id = string.Empty;
        }
    }
}
