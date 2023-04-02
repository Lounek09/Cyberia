namespace Cyberia.Api.Datacenter
{
    [Table("MapSubAreasAudioMusics")]
    public sealed class MapSubAreaAudioMusic : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int MapSubAreaId { get; set; }

        [NotNull]
        public int AudioMusicId { get; set; }

        public MapSubAreaAudioMusic()
        {
            Id = string.Empty;
        }
    }
}
