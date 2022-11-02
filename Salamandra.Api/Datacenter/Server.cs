namespace Salamandra.Api.Datacenter
{
    [Table("Servers")]
    public sealed class Server
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public string Language { get; set; }

        [NotNull]
        public int ServerPopulationId { get; set; }

        [NotNull]
        public int ServerTypeId { get; set; }

        [NotNull]
        public int ServerCommunityId { get; set; }

        [NotNull]
        public int CreationDate { get; set; }

        [NotNull]
        public string RealLanguage { get; set; }

        public Server()
        {
            Name = string.Empty;
            Description = string.Empty;
            Language = string.Empty;
            RealLanguage = string.Empty;
        }
    }
}
