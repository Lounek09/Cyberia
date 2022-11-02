namespace Salamandra.Api.Datacenter
{
    [Table("AlignmentsViewPvpGain")]
    public sealed class AlignmentViewPvpGain
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int TargetAlignmentId { get; set; }

        [NotNull]
        public bool ViewPvpGain { get; set; }

        public AlignmentViewPvpGain()
        {

        }
    }
}
