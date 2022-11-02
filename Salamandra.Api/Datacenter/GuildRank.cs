namespace Salamandra.Api.Datacenter
{
    [Table("GuildRanks")]
    public sealed class GuildRank
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int Order { get; set; }

        [NotNull]
        public int Index { get; set; }

        public GuildRank()
        {
            Name = string.Empty;
        }
    }
}
