using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class AudioMusicContentData
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int AudioMusicId { get; init; }

        [JsonConstructor]
        internal AudioMusicContentData()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioMusicData
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

        [JsonConstructor]
        internal AudioMusicData()
        {
            FileName = string.Empty;
        }
    }

    public sealed class AudioEffectContentData
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int AudioEffectId { get; init; }

        [JsonConstructor]
        internal AudioEffectContentData()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioEffectData
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

        [JsonConstructor]
        internal AudioEffectData()
        {
            FileName = string.Empty;
        }
    }

    public sealed class AudioEnvironmentContentData
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int AudioEnvironmentId { get; init; }

        [JsonConstructor]
        internal AudioEnvironmentContentData()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioEnvironmentData
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

        [JsonConstructor]
        internal AudioEnvironmentData()
        {
            BackgroundAudioEffectIds = [];
            NoiseAudioEffectIds = [];
        }
    }

    public sealed class AudioData
    {
        private const string FILE_NAME = "audio.json";

        [JsonPropertyName("AUMC")]
        public List<AudioMusicContentData> AudioMusicsContent { get; init; }

        [JsonPropertyName("AUM")]
        public List<AudioMusicData> AudioMusics { get; init; }

        [JsonPropertyName("AUEC")]
        public List<AudioEffectContentData> AudioEffectsContent { get; init; }

        [JsonPropertyName("AUE")]
        public List<AudioEffectData> AudioEffects { get; init; }

        [JsonPropertyName("AUAC")]
        public List<AudioEnvironmentContentData> AudioEnvironmentsContent { get; init; }

        [JsonPropertyName("AUA")]
        public List<AudioEnvironmentData> AudioEnvironments { get; init; }

        [JsonConstructor]
        internal AudioData()
        {
            AudioMusicsContent = [];
            AudioMusics = [];
            AudioEffectsContent = [];
            AudioEffects = [];
            AudioEnvironmentsContent = [];
            AudioEnvironments = [];
        }

        internal static AudioData Load()
        {
            return Datacenter.LoadDataFromFile<AudioData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public AudioMusicData? GetAudioMusicDataById(int id)
        {
            return AudioMusics.Find(x => x.Id == id);
        }
    }
}
