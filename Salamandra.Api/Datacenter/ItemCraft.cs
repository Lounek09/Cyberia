namespace Salamandra.Api.Datacenter
{
    [Table("ItemCrafts")]
    public sealed class ItemCraft
    {
        [PrimaryKey, NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public int ComponentItemId { get; set; }

        [NotNull]
        public int Quantity { get; set; }

        public ItemCraft()
        {

        }
    }
}
