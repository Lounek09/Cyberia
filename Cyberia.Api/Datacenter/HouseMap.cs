namespace Cyberia.Api.Datacenter
{
    [Table("HousesMaps")]
    public sealed class HouseMap : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int HouseId { get; set; }

        [NotNull]
        public int MapId { get; set; }

        public HouseMap()
        {
            Id = string.Empty;
        }
    }
}
