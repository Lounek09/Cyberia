namespace Cyberia.Api.Datacenter
{
    [Table("MapAreas")]
    public sealed class MapArea : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int MapSuperAreaId { get; set; }

        public MapArea()
        {
            Name = string.Empty;
        }
    }
}
