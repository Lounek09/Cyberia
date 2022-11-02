namespace Salamandra.Api.Datacenter
{
    [Table("ServerCommunities")]
    public sealed class ServerCommunity
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public bool Visible { get; set; }

        [NotNull]
        public int Index { get; set; }

        [NotNull]
        public string Countries { get; set; }

        public ServerCommunity()
        {
            Name = string.Empty;
            Countries = string.Empty;
        }
    }
}
