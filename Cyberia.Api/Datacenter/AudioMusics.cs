namespace Cyberia.Api.Datacenter
{
    [Table("AudioMusics")]
    public sealed class AudioMusic : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string FileName { get; set; }

        [NotNull]
        public int BaseVolume { get; set; }

        [NotNull]
        public bool Loop { get; set; }

        [NotNull]
        public int Offset { get; set; }

        public AudioMusic()
        {
            Name = string.Empty;
            FileName = string.Empty;
        }
    }
}
