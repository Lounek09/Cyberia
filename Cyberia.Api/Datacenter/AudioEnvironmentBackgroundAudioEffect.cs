namespace Cyberia.Api.Datacenter
{
    [Table("AudioEnvironmentsBackgroundAudioEffects")]
    public sealed class AudioEnvironmentBackgroundAudioEffect : ITable
    {
        [PrimaryKey, NotNull]
        public string Id { get; set; }

        [NotNull]
        public int AudioEnvironmentId { get; set; }

        [NotNull]
        public int AudioEffectId { get; set; }

        public AudioEnvironmentBackgroundAudioEffect()
        {
            Id = string.Empty;
        }
    }
}
