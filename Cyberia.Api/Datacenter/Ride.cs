namespace Cyberia.Api.Datacenter
{
    [Table("Rides")]
    public sealed class Ride : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int GfxId { get; set; }

        [NotNull]
        public int Color1 { get; set; }

        [NotNull]
        public int Color2 { get; set; }

        [NotNull]
        public int Color3 { get; set; }

        public Ride()
        {
            Name = string.Empty;
        }
    }
}
