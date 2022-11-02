namespace Salamandra.Api.Datacenter
{
    [Table("Hints")]
    public sealed class Hint
    {
        [PrimaryKey, NotNull]
        public string Name { get; set; }

        [NotNull]
        public int HintCategoryId { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        [NotNull]
        public int MapId { get; set; }

        public Hint()
        {
            Name = string.Empty;
        }
    }
}
