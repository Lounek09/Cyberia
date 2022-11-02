namespace Salamandra.Api.Datacenter
{
    [Table("AudioEnvironmentsBackgroundAudioEffects")]
    public sealed class AudioEnvironmentBackgroundAudioEffect
    {
        [PrimaryKey, NotNull]
        public int IdAudioEnvironment { get; set; }

        [PrimaryKey, NotNull]
        public int IdAudioEffect { get; set; }

        public AudioEnvironmentBackgroundAudioEffect()
        {

        }
    }
}
