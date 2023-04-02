namespace Cyberia.Api.Datacenter
{
    [Table("AlignmentFeatEffects")]
    public sealed class AlignmentFeatEffect : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Description { get; set; }

        public AlignmentFeatEffect()
        {
            Description = string.Empty;
        }
    }
}
