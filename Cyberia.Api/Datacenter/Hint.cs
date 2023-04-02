namespace Cyberia.Api.Datacenter
{
    [Table("Hints")]
    public sealed class Hint : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
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
