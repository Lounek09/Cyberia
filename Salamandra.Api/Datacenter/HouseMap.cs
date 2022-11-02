namespace Salamandra.Api.Datacenter
{
    [Table("HousesMaps")]
    public sealed class HouseMap
    {
        [PrimaryKey, NotNull]
        public int HouseId { get; set; }

        [PrimaryKey, NotNull]
        public int MapId { get; set; }

        public HouseMap()
        {

        }
    }
}
