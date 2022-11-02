namespace Salamandra.Api.Datacenter
{
    [Table("NearMapSubAreas")]
    public sealed class NearMapSubArea
    {
        [PrimaryKey, NotNull]
        public int MapSubAreaId { get; set; }

        [PrimaryKey, NotNull]
        public int NearMapSubAreaId { get; set; }

        public NearMapSubArea()
        {

        }
    }
}
