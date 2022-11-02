namespace Salamandra.Api.Datacenter
{
    [Table("AlignmentFeatEffects")]
    public sealed class AlignmentFeatEffect
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public AlignmentFeatEffect()
        {
            Name = string.Empty;
        }
    }
}
