namespace Salamandra.Api.Datacenter
{
    [Table("MapSubAreas")]
    public sealed class MapSubArea
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int MapAreaId { get; set; }

        [NotNull]
        public int FightAudioMusicId { get; set; }

        public MapSubArea()
        {
            Name = string.Empty;
        }
    }
}
