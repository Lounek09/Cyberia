namespace Salamandra.Api.Datacenter
{
    [Table("ItemsStats")]
    public sealed class ItemStats
    {
        [PrimaryKey, NotNull] 
        public int Id { get; set; }

        [NotNull]
        public string Stats { get; set; }

        public ItemStats()
        {
            Stats = string.Empty;
        }
    }
}
