namespace Salamandra.Api.Datacenter
{
    [Table("AudioEnvironments")]
    public sealed class AudioEnvironment
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int MinNoiseDelay { get; set; }

        [NotNull]
        public int MaxNoiseDelay { get; set; }

        public AudioEnvironment()
        {

        }
    }
}
