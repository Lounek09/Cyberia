using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class AudioMusicContent
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int AudioMusicId { get; init; }

        public AudioMusicContent()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioMusic
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("f")]
        public string FileName { get; init; }

        [JsonPropertyName("v")]
        public int BaseVolume { get; init; }

        [JsonPropertyName("l")]
        public bool Loop { get; init; }

        [JsonPropertyName("s")]
        public bool Streaming { get; init; }

        [JsonPropertyName("o")]
        public int Offset { get; init; }

        public AudioMusic()
        {
            FileName = string.Empty;
        }
    }

    public sealed class AudioEffectContent
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int AudioEffectId { get; init; }

        public AudioEffectContent()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioEffect
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("f")]
        public string FileName { get; init; }

        [JsonPropertyName("v")]
        public int BaseVolume { get; init; }

        [JsonPropertyName("l")]
        public bool Loop { get; init; }

        [JsonPropertyName("s")]
        public bool Streaming { get; init; }

        [JsonPropertyName("o")]
        public int Offset { get; init; }

        public AudioEffect()
        {
            FileName = string.Empty;
        }
    }

    public sealed class AudioEnvironmentContent
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int AudioEnvironmentId { get; init; }

        public AudioEnvironmentContent()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioEnvironment
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("bg")]
        public List<int> BackgroundAudioEffectIds { get; init; }

        [JsonPropertyName("mind")]
        public int MinNoiseDelay { get; init; }

        [JsonPropertyName("maxd")]
        public int MaxNoiseDelay { get; init; }

        [JsonPropertyName("n")]
        public List<int> NoiseAudioEffectIds { get; init; }

        public AudioEnvironment()
        {
            BackgroundAudioEffectIds = new();
            NoiseAudioEffectIds = new();
        }
    }

    public sealed class AudioData
    {
        private const string FILE_NAME = "audio.json";

        [JsonPropertyName("AUMC")]
        public List<AudioMusicContent> AudioMusicsContent { get; init; }

        [JsonPropertyName("AUM")]
        public List<AudioMusic> AudioMusics { get; init; }

        [JsonPropertyName("AUEC")]
        public List<AudioEffectContent> AudioEffectsContent { get; init; }

        [JsonPropertyName("AUE")]
        public List<AudioEffect> AudioEffects { get; init; }

        [JsonPropertyName("AUAC")]
        public List<AudioEnvironmentContent> AudioEnvironmentsContent { get; init; }

        [JsonPropertyName("AUA")]
        public List<AudioEnvironment> AudioEnvironments { get; init; }

        public AudioData()
        {
            AudioMusicsContent = new();
            AudioMusics = new();
            AudioEffectsContent = new();
            AudioEffects = new();
            AudioEnvironmentsContent = new();
            AudioEnvironments = new();
        }

        internal static AudioData Build()
        {
            return Json.LoadFromFile<AudioData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public AudioMusic? GetAudioMusicById(int id)
        {
            return AudioMusics.Find(x => x.Id == id);
        }
    }
}
