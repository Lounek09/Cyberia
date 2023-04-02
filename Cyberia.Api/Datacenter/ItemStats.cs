namespace Cyberia.Api.Datacenter
{
    [Table("ItemsStats")]
    public sealed class ItemStats : ITable
    {
        [PrimaryKey, NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public string Stats { get; set; }

        public ItemStats()
        {
            Stats = string.Empty;
        }
    }
}
