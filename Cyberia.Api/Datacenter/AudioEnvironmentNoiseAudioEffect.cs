namespace Cyberia.Api.Datacenter
{
    [Table("AudioEnvironmentsNoiseAudioEffects")]
    public sealed class AudioEnvironmentNoiseAudioEffect : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int AudioEnvironmentId { get; set; }

        [NotNull]
        public int AudioEffectId { get; set; }

        public AudioEnvironmentNoiseAudioEffect()
        {
            Id = string.Empty;
        }
    }
}
