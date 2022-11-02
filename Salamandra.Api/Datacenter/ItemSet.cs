namespace Salamandra.Api.Datacenter
{
    [Table("ItemSets")]
    public sealed class ItemSet
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Stats { get; set; }

        public ItemSet()
        {
            Name = string.Empty;
            Stats = string.Empty;
        }
    }
}
