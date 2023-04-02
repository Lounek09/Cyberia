namespace Cyberia.Api.Datacenter
{
    [Table("MapSubAreas")]
    public sealed class MapSubArea : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int MapAreaId { get; set; }

        public MapSubArea()
        {
            Name = string.Empty;
        }
    }
}
