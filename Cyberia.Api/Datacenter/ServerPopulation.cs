namespace Cyberia.Api.Datacenter
{
    [Table("ServerPopulations")]
    public sealed class ServerPopulation : ITable
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
