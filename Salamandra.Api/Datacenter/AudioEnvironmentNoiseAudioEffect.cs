namespace Salamandra.Api.Datacenter
{
    [Table("AudioEnvironmentsNoiseAudioEffects")]
    public sealed class AudioEnvironmentNoiseAudioEffect
    {
        [PrimaryKey, NotNull]
        public int IdAudioEnvironment { get; set; }

        [PrimaryKey, NotNull]
        public int IdAudioEffect { get; set; }

        public AudioEnvironmentNoiseAudioEffect()
        {

        }
    }
}
