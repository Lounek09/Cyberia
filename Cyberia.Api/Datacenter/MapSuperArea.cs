namespace Cyberia.Api.Datacenter
{
    [Table("MapSuperAreas")]
    public sealed class MapSuperArea : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public MapSuperArea()
        {
            Name = string.Empty;
        }
    }
}
