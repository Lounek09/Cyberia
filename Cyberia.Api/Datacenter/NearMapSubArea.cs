namespace Cyberia.Api.Datacenter
{
    [Table("NearMapSubAreas")]
    public sealed class NearMapSubArea : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int MapSubAreaId { get; set; }

        [NotNull]
        public int NearMapSubAreaId { get; set; }

        public NearMapSubArea()
        {
            Id = string.Empty;
        }
    }
}
