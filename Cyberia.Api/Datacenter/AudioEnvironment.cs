namespace Cyberia.Api.Datacenter
{
    [Table("AudioEnvironments")]
    public sealed class AudioEnvironment : ITable
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int MinNoiseDelay { get; set; }

        [NotNull]
        public int MaxNoiseDelay { get; set; }

        public AudioEnvironment()
        {
            Name = string.Empty;
        }
    }
}
