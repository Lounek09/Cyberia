namespace Cyberia.Api.Datacenter
{
    [Table("ItemSetsItems")]
    public sealed class ItemSetItem : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int ItemSetId { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        public ItemSetItem()
        {
            Id = string.Empty;
        }
    }
}
