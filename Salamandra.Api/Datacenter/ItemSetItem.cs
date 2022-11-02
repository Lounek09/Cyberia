namespace Salamandra.Api.Datacenter
{
    [Table("ItemSetsItems")]
    public sealed class ItemSetItem
    {
        [PrimaryKey, NotNull]
        public int ItemSetId { get; set; }

        [PrimaryKey, NotNull]
        public int ItemId { get; set; }

        public ItemSetItem()
        {

        }
    }
}
