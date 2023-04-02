namespace Cyberia.Api.Datacenter
{
    [Table("Houses")]
    public sealed class House : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        public int? OutdoorMapId { get; set; }

        public int? NumberOfRooms { get; set; }

        public int? NumberOfChests { get; set; }

        public int? Price { get; set; }

        public House()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
