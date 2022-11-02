namespace Salamandra.Api.Datacenter
{
    [Table("ServerPopulations")]
    public sealed class ServerPopulation
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int Weight { get; set; }

        public ServerPopulation()
        {
            Name = string.Empty;
        }
    }
}
