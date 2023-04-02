namespace Cyberia.Api.Datacenter
{
    [Table("ItemCrafts")]
    public sealed class ItemCraft : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public int ComponentItemId { get; set; }

        [NotNull]
        public int Quantity { get; set; }

        public ItemCraft()
        {
            Id = string.Empty;
        }
    }
}
