namespace Cyberia.Api.Datacenter
{
    [Table("Emotes")]
    public sealed class Emote : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Shortcut { get; set; }

        public Emote()
        {
            Name = string.Empty;
            Shortcut = string.Empty;
        }
    }
}
